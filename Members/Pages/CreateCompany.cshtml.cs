using System.Threading.Tasks;
using Members.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement.Notify;
namespace Members.Pages
{
    public class CreateCompanyModel : PageModel
    {
        private readonly IHtmlLocalizer H;
        private readonly MemberService _mService;
        private readonly INotifier _notifier;

        public dynamic Shape { get; set; }

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
            var member = await _mService.GetUserMember();

            if (member == null)
            {
                return RedirectToPage("CreateMember");
            }

            (_, Shape) = await _mService.GetNewItem(MemberType.Company);
            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {

            ContentItem contentItem;
            (contentItem, Shape) = await _mService.GetUpdatedItem(MemberType.Company);
            if (ModelState.IsValid)
            {
                var result = await _mService.CreateMemberCompany(contentItem);
                if (result.Succeeded)
                {
                    _notifier.Success(H["Legal entity added successfully"]);
                    return RedirectToPage("Portal");
                }
            }
            return Page();
        }

    }
}
