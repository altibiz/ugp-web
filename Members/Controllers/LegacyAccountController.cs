using Members.Base;
using Members.Core;
using Members.Persons;
using Members.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.Users;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using YesSql;

namespace Members.Controllers
{
    [ApiController]
    [Route("api/legacyaccount")]
    [Authorize(AuthenticationSchemes = "Api"), IgnoreAntiforgeryToken, AllowAnonymous]
    public class LegacyAccountController : Controller
    {
        private readonly UserManager<IUser> _userManager;
        private readonly ISession _session;
        private readonly IUserService _userService;

        public LegacyAccountController(UserManager<IUser> userManager, ISession session, IUserService userService)
        {
            _userManager = userManager;
            _session = session;
            _userService = userService;
        }


        public class LegacyLoginModel
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(LegacyLoginModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var user = await _userManager.FindByNameAsync(model.UserName) ?? await _userManager.FindByEmailAsync(model.UserName);

            if (user == null)
            {

                var oldLoginItem = await _session.Query<ContentItem, PersonPartIndex>(x => x.Oib == model.UserName).FirstOrDefaultAsync();
                ContentItem memberItem = oldLoginItem;
                if (oldLoginItem?.ContentType == "Company")
                {
                    memberItem = await _session.GetListItemParent(oldLoginItem);
                }
                if (oldLoginItem == null || memberItem == null) return Ok(model.UserName);
                var mem = memberItem?.As<Member>().InitFields();
                var memPerson = memberItem.As<PersonPart>();
                var oldPerson = oldLoginItem.As<PersonPart>();

                if (mem.User?.UserIds?.Length > 0)
                {
                    var usr = await _userService.GetUserByUniqueIdAsync(mem.User.UserIds[0]);
                    if (usr != null)
                    {
                        model.UserName = usr.UserName;
                        return Ok(usr.UserName);
                    }
                }

                var email = memPerson?.Email?.Text
                                ?? oldPerson?.Email?.Text;

                if (string.IsNullOrWhiteSpace(email))
                    return ValidationProblem("Član nema upisanu email adresu, molim kontaktirajte udrugu sa svojim OIB-om");
                var newusrname = memPerson?.Oib?.Text ?? oldPerson?.Oib?.Text
                    ?? email?.Replace("@", "_");

                var passWord = newusrname + Guid.NewGuid();
                if (!string.IsNullOrEmpty(oldPerson.OldHash) && !string.IsNullOrEmpty(oldPerson.OldSalt))
                {
                    string thHash = GetHash(model.Password, oldPerson.OldSalt);
                    if (thHash == oldPerson.OldHash)
                    {
                        passWord = model.Password;
                    }
                }

                var newuser = await _userManager.FindByNameAsync(newusrname)
                    ?? await _userManager.FindByEmailAsync(email)
                    ?? await _userService.CreateUserAsync(new User { UserName = newusrname, Email = email, EmailConfirmed = true, IsEnabled = true }, passWord, (key, message) => ModelState.AddModelError(key, message));

                mem.User.UserIds = new[] { (newuser as User).UserId };
                memberItem.Apply(mem);
                _session.Save(memberItem);
                return Ok(email ?? newusrname);
            }
            return Ok(model.UserName);
        }

        public static string GetHash(string password, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
            SHA256Managed hashstring = new();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }
    }
}
