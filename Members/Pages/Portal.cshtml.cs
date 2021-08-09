using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Members.Core;

namespace Members.Pages
{
    [Authorize]
    public class PortalModel : PageModel
    {
        private MemberService _mService;

        public PortalModel(MemberService mService)
        {
            _mService = mService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var member = await _mService.GetUserMember();

            if (member == null)
            {
                return RedirectToPage("CreateMember");
            }
            return Page();
        }
    }
}