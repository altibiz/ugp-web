using System.Threading.Tasks;
using Members.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Notify;

namespace Members.Pages
{
    [Authorize]
    public class CreateMemberModel : PageModel
    {
        private readonly IHtmlLocalizer H;
        private readonly MemberService _memberService;
        private readonly INotifier _notifier;

        public IShape Shape { get; set; }

        public CreateMemberModel(MemberService mService, IHtmlLocalizer<CreateMemberModel> htmlLocalizer, INotifier notifier)
        {

            _notifier = notifier;
            H = htmlLocalizer;
            _memberService = mService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var mbr = await _memberService.GetUserMember(true);
            if (mbr != null) return RedirectToPage("Portal");
            (_, Shape) = await _memberService.GetNewItem(ContentType.Member);
            return Page();
        }


        public async Task<IActionResult> OnPostCreateMemberAsync()
        {
            return await CreatePOST("Portal");
        }
        public async Task<IActionResult> OnPostCreateMemberToCompanyAsync()
        {
            return await CreatePOST("CreateCompany");
        }

        private async Task<IActionResult> CreatePOST(string nextPage)
        {
            ContentItem contentItem;
            (contentItem, Shape) = await _memberService.ModelToNew(ContentType.Member);
            if (ModelState.IsValid) {
                var result = await _memberService.CreateMemberDraft(contentItem);
                if (result.Succeeded)
                {
                    await _notifier.SuccessAsync(H["Member registration successful"]);
                    return RedirectToPage(nextPage);
                }
            }
            return Page();

        }
    }
}
