using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using YesSql;
using OrchardCore;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.DisplayManagement.Notify;
using Members.Core;

namespace Members.Pages
{
    public class MyCompanyModel : PageModel
    {
        private readonly IContentManager _contentManager;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;

        private readonly IUpdateModelAccessor _updateModelAccessor;

        private readonly IHtmlLocalizer H;
        private readonly MemberService _memberService;
        private readonly INotifier _notifier;

        public dynamic Shape { get; set; }

        public MyCompanyModel(IContentManager contentManager, IContentItemDisplayManager contentItemDisplayManager, IUpdateModelAccessor updateModelAccessor,
            MemberService mService, IHtmlLocalizer<CreateMemberModel> htmlLocalizer, INotifier notifier)
        {
            _contentManager = contentManager;
            _contentItemDisplayManager = contentItemDisplayManager;

            _updateModelAccessor = updateModelAccessor;

            _notifier = notifier;
            H = htmlLocalizer;
            _memberService = mService;
        }


        public async Task OnGetAsync(string companyId)
        {
            var company = await _contentManager.GetAsync(companyId);

            Shape = await _contentItemDisplayManager.BuildEditorAsync(company, _updateModelAccessor.ModelUpdater, true);

        }

        public async Task<IActionResult> OnPostUpdateCompanyAsync()
        {
            return await UpdatePOST("MyCompany");
        }

        private async Task<IActionResult> UpdatePOST(string nextPage)
        {
            ContentItem contentItem;
            (contentItem, Shape) = await _memberService.GetUpdatedItem(ContentType.Company);

            if (ModelState.IsValid)
            {
                var result = await _memberService.UpdateMemberCompany(contentItem);

                if (result.Succeeded)
                {
                    _notifier.Success(H["Company updated successful"]);
                    return RedirectToPage(nextPage+"?companyID="+contentItem.ContentItemId);
                }
            }
            Shape = await _contentItemDisplayManager.BuildEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);

            return Page();
        }
    }
}