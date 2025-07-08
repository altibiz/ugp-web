using Members.Base;
using Members.Core;
using Members.PartFieldSettings;
using Members.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YesSql.Services;

namespace Members.Payments
{
    public class PledgeService : PartService<Pledge>
    {
        private MemberService _memberService;

        public IStringLocalizer<PledgeService> S { get; }

        public PledgeService(IHttpContextAccessor htp, MemberService memberService, IStringLocalizer<PledgeService> s) : base(htp)
        {
            _memberService = memberService;
            S = s;
        }

        public override IEnumerable<ValidationResult> Validate(Pledge part)
        {
            if (!string.IsNullOrWhiteSpace(part.Oib?.Text))
            {
                var oib = part.Oib.Text;
                if (string.IsNullOrEmpty(oib) || oib.Count(char.IsDigit) < 5)
                {
                    yield return new ValidationResult(S["ID must have numbers"], [nameof(part.Oib)]);
                }
            }
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
                if (member != null)
                    model.Person.SetId(member.ContentItemId);
            }
        }
    }
}
