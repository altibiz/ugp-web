using Members.Base;
using Members.Core;
using Members.PartFieldSettings;
using Members.Persons;
using Members.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
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

        private IHttpContextAccessor _htp;

        public PledgeService(IHttpContextAccessor htp, MemberService memberService, IStringLocalizer<PledgeService> s) : base(htp)
        {
            _memberService = memberService;
            S = s;
            _htp = htp;
        }

        public override async Task InitializingAsync(Pledge part)
        {
            if (IsAdmin) return;
            var cid = Context.Request.Query["initPledgePersonCid"];
            var current = string.IsNullOrEmpty(cid) ? await _memberService.GetUserMember(true) :
                await _memberService.GetContentItemById(cid);
            if (current != null)
            {
                var person = current.AsInit<PersonPart>();
                part.Person.SetId(current.ContentItemId);
                part.Oib.Text = person.Oib.Text;
                part.PayerName.Text = person.LegalName;
                part.Email.Text = person.Email.Text;
            }
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
            var httpContext = _htp.HttpContext;
            if (!IsAdmin)
            {
                var variant = await model.Variant.GetTerm(Context);
                var variantPart = variant?.AsInit<PledgeVariant>();
                model.ReferenceNr.Text = (variantPart?.ReferenceNrPrefix?.Text ?? "") + model.Oib.Text;
                model.Amount.Value = variantPart?.Price.Value;
                model.Note.Text = variant?.DisplayText;
                var member = await _memberService._session.GetByOib(model.Oib.Text);
                if (member != null)
                    model.Person.SetId(member.ContentItemId);
            }
        }
    }
}
