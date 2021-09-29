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
        public ContentItem ContentItem { get; set; }
        public string DocLink { get; set; }
        public MyCompanyModel(MemberService mService, IHtmlLocalizer<CreateMemberModel> htmlLocalizer, INotifier notifier)
        {
            _notifier = notifier;
            H = htmlLocalizer;
            _memberService = mService;
        }
                
        public async Task OnGetAsync(string companyId)
        {

            ContentItem company = await _memberService.GetContentItemById(companyId);

            (Shape, ContentItem) = await _memberService.GetEditorById(companyId);

            var docUrl = string.Format("https://{0}/Members/Print/{1}" , Request.Host, companyId);

            var fileName = "Membership-" + company.DisplayText;

            DocLink = string.Format("https://altibizhtml2pdf.azurewebsites.net/api/generatePdf?code=bPJSYSigg05QxJvYAsqPr584dwV897UbUialY99kTzNvtEt5lzBSkw==&fileName={0}&url={1}", fileName, docUrl);
        }

        public async Task<IActionResult> OnPostAsync(string companyId)
        {
            ContentItem contentItem;
            (contentItem, Shape) = await _memberService.GetUpdatedItem(companyId);

            if (ModelState.IsValid)
            {
                var result = await _memberService.UpdateContentItem(contentItem);

                if (result.Succeeded)
                    _notifier.Success(H["Company updated successful"]);

                return RedirectToPage("MyCompany", new { companyId = companyId });

            }
            return Page();
        }
    }
}