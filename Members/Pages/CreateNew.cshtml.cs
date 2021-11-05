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
    public class CreateNewModel : PageModel
    {
        private readonly IHtmlLocalizer H;
        private readonly MemberService _memberService;
        private readonly INotifier _notifier;
        public IShape Shape { get; set; }
        public CreateNewModel(MemberService mService, IHtmlLocalizer<CreateOfferModel> htmlLocalizer, INotifier notifier)
        {

            _notifier = notifier;
            H = htmlLocalizer;
            _memberService = mService;
        }

        public async Task<IActionResult> OnGetAsync(string contentType)
        {
            (_, Shape) = await _memberService.GetNewItem(contentType);
            return Page();
        }

        // contentItemId -> company content item
        public async Task<IActionResult> OnPostAsync(string contentType)
        {
            ContentItem contentItem;
            (contentItem, Shape) = await _memberService.ModelToNew(contentType);
            if (ModelState.IsValid)
            {
                var result = await _memberService.CreateNew(contentItem,true);
                if (result.Succeeded)
                {
                    await _notifier.SuccessAsync(H["Hvala!"]);
                    return Redirect("~/Contents/ContentItems/"+contentItem.ContentItemId);
                }
            }
            return Page();
        }

    }
}
