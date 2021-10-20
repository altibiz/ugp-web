using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Members.Base;
using Members.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentFields.Indexing.SQL;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement.Notify;
using YesSql;

namespace Members.Pages
{
    public class OffersForMembersModel : PageModel
    {
        private readonly IHtmlLocalizer H;
        private readonly MemberService _memberService;
        private readonly INotifier _notifier;
        private readonly ISession _session;

        public List<ContentItem> OfferContentItems { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public List<LogoUrl> Logos { get; set; }

        public OffersForMembersModel(ISession session, MemberService mService, IHtmlLocalizer<CreateMemberModel> htmlLocalizer, INotifier notifier)
        {
            _notifier = notifier;
            H = htmlLocalizer;
            _memberService = mService;
            _session = session;

        }

        public async Task OnGetAsync(string catId = null)
        {
            Logos = new List<LogoUrl>();
            if (catId != null)
            {
                OfferContentItems = await _memberService.GetAllOffersByTag(catId);
            }
            else
            {
                OfferContentItems = await _memberService.GetAllOffers();
            }

            if (SearchString != null)
            {
                OfferContentItems = await _memberService.GetAllOffersSearch(SearchString);
            }
            foreach (var item in OfferContentItems.AsParts<Offer>())
            {
                var cid = item.Company.GetId();
                if (cid == null) continue;
                
                ContentItem company = await _memberService.GetContentItemById(cid);
                var companyPart = company.As<Company>();
                LogoUrl log = new();
                log.CompanyID = cid;
                log.Url = companyPart.Logo?.Paths?.FirstOrDefault();
                Logos.Add(log);
            }
        }
        public async Task<IEnumerable<ContentItem>> GetTextFieldIndexRecords(string contentType, string contentField)
        {
            return await _session.Query<ContentItem, TextFieldIndex>(x => x.ContentType == contentType && x.ContentField == contentField).ListAsync();
        }
    }
    public class LogoUrl
    {
        public string CompanyID { get; set; }
        public string Url { get; set; }
    }
}
