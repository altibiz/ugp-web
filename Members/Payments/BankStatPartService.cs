using Members.Base;
using Members.Core;
using Members.PartFieldSettings;
using Members.Persons;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Handlers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Members.Payments
{
    class BsJson
    {
        public class Stat
        {
            public class Rrn
            {
                public string Model { get; set; }
                public string Number { get; set; }
            }
            public class CPartner
            {
                public class CAddress
                {
                    public string Street { get; set; }
                    public string City { get; set; }
                }
                public string Name { get; set; }
                public CAddress Address { get; set; }
            }
            public int BankID { get; set; }
            public decimal Amount { get; set; }

            public string Type { get; set; }
            public Rrn RRN { get; set; }
            public CPartner Partner { get; set; }
        }
        public Stat[] Data { get; set; }

    }

    public class BankStatPartService : PartService<BankStatPart>
    {
        public BankStatPartService()
        {

        }

        public IStringLocalizer<BankStatPartService> S { get; }

        private IContentManager _contentManager;
        private PersonPartService _pService;

        public BankStatPartService(IStringLocalizer<BankStatPartService> S, IContentManager contentManager, PersonPartService personService)
        {
            this.S = S;
            _contentManager = contentManager;
            _pService = personService;
        }

        public override Action<BankStatPart> GetEditModel(BankStatPart part, BuildPartEditorContext context)
        {
            return m => { m.StatementJson = part.StatementJson; };
        }

        public override Task InitializingAsync(BankStatPart part)
        {
            part.Date = new DateField() { Value = DateTime.Now };
            return Task.CompletedTask;
        }

        public async override IAsyncEnumerable<ValidationResult> ValidateAsync(BankStatPart part)
        {
            if (string.IsNullOrEmpty(part.StatementJson))
            {
                yield return new ValidationResult(S["Statement is required."], new[] { nameof(part.StatementJson) });
            }
            var parsed = true;
            try
            {
                JsonConvert.DeserializeObject<BsJson>(part.StatementJson);
            }
            catch
            {
                parsed = false;
            }
            if (!parsed)
                yield return new ValidationResult(S["Statement not a valid json"], new[] { nameof(part.StatementJson) });
        }

        public async override Task PublishedAsync(BankStatPart part, PublishContentContext context)
        {
            var json = JsonConvert.DeserializeObject<BsJson>(part.StatementJson);
            foreach (var pymnt in json.Data.Where(x => x.Type == "Uplata"))
            {
                var ciPayment = await _contentManager.NewAsync("Payment");
                var payPart = ciPayment.As<Payment>().InitFields();
                payPart.Amount.Value = pymnt.Amount;
                payPart.Address.Text = pymnt.Partner.Address.Street;
                payPart.PayerName.Text = pymnt.Partner.Name;
                payPart.PaymentRef.Text = pymnt.RRN.Number;
                var version = VersionOptions.Draft;
                if (pymnt.RRN.Number?.Length >= 11)
                {
                    var person = (await _pService.GetByOibAsync(pymnt.RRN.Number.Substring(pymnt.RRN.Number.Length - 11))).FirstOrDefault();
                    if (person != null)
                    {
                        payPart.Person.ContentItemIds = new[] { person.ContentItemId };
                        version = VersionOptions.Published;
                    }
                }
                ciPayment.Apply(payPart);
                await _contentManager.UpdateValidateAndCreateAsync(ciPayment, version);
            }
        }
    }
}
