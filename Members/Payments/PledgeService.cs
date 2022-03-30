using Members.Base;
using Members.Core;
using Members.PartFieldSettings;
using Members.Utils;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using System.Threading.Tasks;

namespace Members.Payments
{
    public class PledgeService:PartService<Pledge>
    {
        private MemberService _memberService;

        public PledgeService(IHttpContextAccessor htp,MemberService memberService):base(htp)
        {
            _memberService = memberService;
        }
        public override async Task UpdatedAsync(UpdateContentContext context, Pledge model)
        {
            model.InitFields();
            if (!IsAdmin)
            {
                model.ReferenceNr.Text = "11-" + model.Oib.Text;
                var variant = await model.Variant.GetTerm(Context);
                model.Amount.Value = variant.As<PledgeVariant>().Price.Value;
                model.Note.Text = variant.DisplayText;
                var member = await _memberService.GetByOib(model.Oib.Text);
                if(member!=null)
                    model.Person.SetId(member.ContentItemId);
            }
        }
    }
}
