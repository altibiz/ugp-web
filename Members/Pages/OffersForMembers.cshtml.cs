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
    public class OffersForMembersModel : PageModel
    {
        private readonly IHtmlLocalizer H;
        private readonly MemberService _memberService;
        private readonly INotifier _notifier;
        public List<ContentItem> OfferContentItems { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public OffersForMembersModel(MemberService mService, IHtmlLocalizer<CreateMemberModel> htmlLocalizer, INotifier notifier)
        {
            _notifier = notifier;
            H = htmlLocalizer;
            _memberService = mService;
        }

        public async Task OnGetAsync(string catId = null)
        {

            if (catId!=null)
            {
                OfferContentItems = await _memberService.GetOffersForUserByTag(catId);
            }
            else
            {
                OfferContentItems = await _memberService.GetOffersForUser();
            }

            if (SearchString != null)
            {
                OfferContentItems = (List<ContentItem>)OfferContentItems.Where(x => x.Content.Offer.DisplayText.Text.Contains(SearchString)).ToList();
            }
        }
    }
}
