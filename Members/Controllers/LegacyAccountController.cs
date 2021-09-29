﻿using Members.Base;
using Members.Core;
using Members.Persons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.Users;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using OrchardCore.Users.ViewModels;
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
        private UserManager<IUser> _userManager;
        private ISession _session;
        private IUserService _userService;

        public LegacyAccountController(UserManager<IUser> userManager, ISession session, IUserService userService)
        {
            _userManager = userManager;
            _session = session;
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Post(LoginViewModel model)
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
                    memberItem = await _session.GetListParent(oldLoginItem);
                }

                var mem = memberItem.As<Member>().InitFields();
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

                var newusrname = memPerson?.Oib?.Text ?? oldPerson?.Oib?.Text 
                    ?? email?.Replace("@","_");

                var passWord = newusrname+Guid.NewGuid();
                if (!string.IsNullOrEmpty(oldPerson.OldHash) && !string.IsNullOrEmpty(oldPerson.OldSalt))
                {
                    string thHash = GetHash(model.Password, oldPerson.OldSalt);
                    if (thHash == oldPerson.OldHash)
                    {
                        passWord = model.Password;
                    }
                }

                var newuser = await _userService.CreateUserAsync(new User { UserName = newusrname, Email = email, EmailConfirmed = true, IsEnabled = true }, passWord, (key, message) => ModelState.AddModelError(key, message));

                mem.User.UserIds = new[] { (newuser as User).UserId };
                memberItem.Apply(mem);
                _session.Save(memberItem);
                return Ok(email ?? newusrname);
            }
            return Ok(model.UserName);
        }

        public string GetHash(string password, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
            SHA256Managed hashstring = new SHA256Managed();
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