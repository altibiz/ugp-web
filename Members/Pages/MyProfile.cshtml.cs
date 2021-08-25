using System.Collections.Generic;
using System.Threading.Tasks;
using Members.Core;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;

namespace Members.Pages
{
    public class MyProfileModel : PageModel
    {
        private const string contentType = "Member";


        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IHtmlLocalizer H;
        private readonly IUpdateModelAccessor _updateModelAccessor;
        private readonly MemberService _memberService;

        public dynamic Shape { get; set; }
        public List<ContentItem> CompanyContentItems { get; set; }

        public MyProfileModel(MemberService mService, IContentItemDisplayManager contentItemDisplayManager, IHtmlLocalizer<CreateMemberModel> htmlLocalizer, IUpdateModelAccessor updateModelAccessor)
        {
            _contentItemDisplayManager = contentItemDisplayManager;
            _updateModelAccessor = updateModelAccessor;

            H = htmlLocalizer;

            _memberService = mService;

        }

        public async Task OnGetAsync()
        {
            var member = await _memberService.GetUserMember();

            CompanyContentItems = await _memberService.GetUserCompanies(member.ContentItemId,true);

            Shape = await _contentItemDisplayManager.BuildEditorAsync(member, _updateModelAccessor.ModelUpdater, false);
        }
    }
}
