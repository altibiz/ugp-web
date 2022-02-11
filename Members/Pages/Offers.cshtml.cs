using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Members.Base;
using Members.Core;
using Members.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentFields.Indexing.SQL;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement.Notify;
using YesSql;

namespace Members.Pages
{
    [Authorize]
    public class OffersModel : PageModel
    {
        private readonly MemberService _memberService;
        private readonly ISession _session;

        public List<ContentItem> OfferContentItems { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public List<LogoUrl> Logos { get; set; }

        public OffersModel(ISession session, MemberService mService)
        {
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
                if (company == null) return;
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
