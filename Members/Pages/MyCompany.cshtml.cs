using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.DisplayManagement.Notify;
using Members.Core;
using OrchardCore.DisplayManagement;

namespace Members.Pages
{
    public class MyCompanyModel : PageModel
    {
        private readonly IHtmlLocalizer H;
        private readonly MemberService _memberService;
        private readonly INotifier _notifier;

        public IShape Shape { get; set; }

        public MyCompanyModel(MemberService mService, IHtmlLocalizer<CreateMemberModel> htmlLocalizer, INotifier notifier)
        {
            _notifier = notifier;
            H = htmlLocalizer;
            _memberService = mService;
        }
                
        public async Task OnGetAsync(string companyId)
        {
            Shape = await _memberService.GetEditorById(companyId);
        }

        public async Task<IActionResult> OnPostAsync(string companyId)
        {
            ContentItem contentItem;
            (contentItem, Shape) = await _memberService.GetUpdatedItem(companyId);

            if (ModelState.IsValid)
            {
                var result = await _memberService.UpdateMemberCompany(contentItem);

                if (result.Succeeded)
                    _notifier.Success(H["Company updated successful"]);
            }
            return Page();
        }
    }
}