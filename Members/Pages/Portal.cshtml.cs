using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Members.Core;
using OrchardCore.DisplayManagement.Notify;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.ContentManagement;

namespace Members.Pages
{
    [Authorize]
    public class PortalModel : PageModel
    {
        private MemberService _mService;
        private INotifier _notifier;
        private IHtmlLocalizer<PortalModel> H;
        public ContentItem Member;
        public PortalModel(MemberService mService, INotifier notifier, IHtmlLocalizer<PortalModel> localizer)
        {
            _mService = mService;
            _notifier = notifier;
            H = localizer;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            Member = await _mService.GetUserMember(true);

            if (Member == null)
            {
                return RedirectToPage("CreateMember");
            }
            if (!Member.Published)
            {
                _notifier.Information(H["Molimo pričekajte da naši administratori potvrde prijavu"]);
            }
            return Page();
        }
    }
}