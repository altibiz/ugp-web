using Members.Base;
using Members.Core;
using Members.PartFieldSettings;
using Members.Persons;
using Members.Utils;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using System.Threading.Tasks;

namespace Members.Payments
{
    public class PaymentPartService : PartService<Payment>
    {
        private MemberService _memberService;
        private IContentManager _contentManager;

        public PaymentPartService(IHttpContextAccessor httpAccessorService, MemberService service,IContentManager contentManager) : base(httpAccessorService)
        {
            _memberService = service;
            _contentManager= contentManager;
        }

        public override Task UpdatedAsync(UpdateContentContext context, Payment instance)
        {
            if (instance.IsPayout.Value ^ instance.Amount.Value < 0)
                instance.Amount.Value = -instance.Amount.Value;
            return Task.CompletedTask;
        }

        public async override Task PublishedAsync(Payment instance, PublishContentContext context)
        {
            var memberOrCompany = await _memberService.GetContentItemById(instance.Person.GetId());
            if (memberOrCompany != null 
                && instance.Amount.Value.GetValueOrDefault() >= Constants.MembershipMinAmount
                && instance.IsPayout.Value == false
                && (StringUtils.ContainsAny(instance.Description.Text, ["ćlanar", "clanar", "members", "članar"])
                || (instance.ReferenceNr.Text?.StartsWith(Constants.ReferenceMembershipPrefix) ?? false)
                || (instance.ReferenceNr.Text?.StartsWith("HR00"+Constants.ReferenceMembershipPrefix) ?? false)
                ))
            {
                memberOrCompany.AlterInit<PersonPart>(p =>
                {
                    if (p.MembershipExpiry.Value.GetValueOrDefault() < instance.Date.Value.GetValueOrDefault().AddYears(1))
                    {
                        p.MembershipExpiry.Value = instance.Date.Value.GetValueOrDefault().AddYears(1);
                    }
                });
                await _contentManager.UpdateAsync(memberOrCompany);
            }
        }


    }
}