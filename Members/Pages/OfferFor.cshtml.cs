using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Members.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;

namespace Members.Pages
{
    public class OfferForModel : PageModel
    {
        private readonly IHtmlLocalizer H;
        private readonly IContentManager _contentManager;
        private readonly INotifier _notifier;
        private readonly MemberService _memberService;
        public List<ContentItem> CompanyContentItems { get; set; }

        public OfferForModel(IContentManager cManager, MemberService mService, IHtmlLocalizer<CreateMemberModel> htmlLocalizer, INotifier notifier)
        {
            _notifier = notifier;
            H = htmlLocalizer;
            _contentManager = cManager;
            _memberService = mService;
        }

        public async Task OnGetAsync()
        {
            CompanyContentItems = await _memberService.GetUserCompanies();
        }

    }
}
