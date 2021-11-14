using System.Threading.Tasks;
using Members.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Notify;
namespace Members.Pages
{
    public class CreateCompanyModel : PageModel
    {
        private readonly IHtmlLocalizer H;
        private readonly MemberService _mService;
        private readonly INotifier _notifier;

        public IShape Shape { get; set; }

        public CreateCompanyModel(MemberService mService,
            IHtmlLocalizer<CreateCompanyModel> htmlLocalizer,
            INotifier notifier)
        {

            _notifier = notifier;

            H = htmlLocalizer;
            _mService = mService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var member = await _mService.GetUserMember(true);

            if (member == null)
            {
                return RedirectToPage("CreateMember");
            }

            (_, Shape) = await _mService.GetNewItem(ContentType.Company);
            return Page();

        }

        public async Task<IActionResult> OnPostAsync(string returnPage)
        {
            ContentItem contentItem;
            (contentItem, Shape) = await _mService.ModelToNew(ContentType.Company);
            if (ModelState.IsValid)
            {
                var result = await _mService.CreateMemberCompany(contentItem);
                if (result.Succeeded)
                {
                    await _notifier.SuccessAsync(H["Legal entity added successfully"]);
                    return RedirectToPage(returnPage ?? "Portal");
                }
            }
            return Page();
        }

    }
}
