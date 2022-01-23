using Members.Core;
using OrchardCore.ContentManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using YesSql;

namespace Members.Payments
{
    public class PaymentUtils
    {
        private MemberService _memService;
        private ISession _session;

        public PaymentUtils(MemberService memberService,ISession session)
        {
            _memService = memberService;
            _session = session;
        }
        public async IAsyncEnumerable<Payment> GetUserPayments()
        {
            var member = await _memService.GetUserMember();
            var companies = await _memService.GetUserCompanies();
            foreach (var payment in await GetPersonPayments(member.ContentItemId))
                yield return payment.As<Payment>();
            foreach (var comp in companies)
                foreach (var payment in await GetPersonPayments(comp.ContentItemId))
                    yield return payment.As<Payment>();
        }

        internal async Task<IEnumerable<ContentItem>> GetPersonPayments(string contentItemId)
        {
            return await _session.Query<ContentItem, PaymentIndex>(x => x.PersonContentItemId == contentItemId).ListAsync();
        }
    }
}
