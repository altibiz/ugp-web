using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Users.Services;
using YesSql;
using OrchardCore;
using OrchardCore.ContentFields.Indexing.SQL;
using OrchardCore.Users;
using Microsoft.AspNetCore.Mvc;

namespace Members.Pages
{
    public class PortalModel : PageModel
    {
        private const string memberType = "Member";

        private readonly ISession _session;
        private readonly IUserService _userService;

        public PortalModel(ISession session, IUserService userService)
        {
            _userService = userService;
            _session = session;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userService.GetAuthenticatedUserAsync(User) as OrchardCore.Users.Models.User;

            var query = _session.Query<ContentItem, UserPickerFieldIndex>();
            query = query.With<UserPickerFieldIndex>(x => x.ContentType == memberType && x.Published && x.SelectedUserId==user.UserId);

            
            var member = await query.ListAsync();

            if (member.Count()==0)
            {
                return RedirectToPage("CreateMember");
            }
            return Page();
        }
    }
}