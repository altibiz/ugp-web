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
    public class OfferForMembersModel : PageModel
    {
        private readonly IHtmlLocalizer H;
        private readonly MemberService _memberService;
        private readonly INotifier _notifier;
        public List<ContentItem> OfferContentItems { get; set; }

        public OfferForMembersModel(MemberService mService, IHtmlLocalizer<CreateMemberModel> htmlLocalizer, INotifier notifier)
        {
            _notifier = notifier;
            H = htmlLocalizer;
            _memberService = mService;
        }

        public async Task OnGetAsync()
        {
            OfferContentItems = await _memberService.GetOffersForUser();
        }

    }
}
