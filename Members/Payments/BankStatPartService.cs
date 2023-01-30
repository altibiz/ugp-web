using Members.Base;
using Members.PartFieldSettings;
using Members.Persons;
using Members.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Handlers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Members.Payments
{
    public class BsJson
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
            public DateTime? Date { get; set; }
            public int BankID { get; set; }
            public decimal Amount { get; set; }
            public string PaymentDescription { get; set; }
            public string Type { get; set; }
            public Rrn RRN { get; set; }
            public CPartner Partner { get; set; }
            public string Number { get; set; }
        }

        public List<Stat> Data { get; set; }
        public DateTime? Date { get; set; }
    }
    public class BankStatPartService : PartService<BankStatPart>
    {

        public IStringLocalizer<BankStatPartService> S { get; }

        private readonly IContentManager _contentManager;
        private readonly PersonPartService _pService;
        private readonly YesSql.ISession _session;

        public BankStatPartService(IStringLocalizer<BankStatPartService> S, IContentManager contentManager, PersonPartService personService, IHttpContextAccessor htp, YesSql.ISession session) : base(htp)
        {
            this.S = S;
            _contentManager = contentManager;
            _pService = personService;
            _session = session;
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

        public static BsJson ParseStmt(string xmlOrJson)
        {
            try
            {
                var res = JsonConvert.DeserializeObject<BsJson>(xmlOrJson);
                res.Date = res.Data.FirstOrDefault()?.Date;
                return res;
            }
            catch
            {

            }
            //if it continues, try xml;

            XmlDocument doc = new();
            doc.LoadXml(xmlOrJson);
            xmlOrJson = Regex.Replace(xmlOrJson, "<Document.*?>", "<Document>"); //get rid of namespaces
                                                                                 //new test
            XElement document = XElement.Parse(xmlOrJson);

            BsJson statement = new();
            statement.Data = new List<BsJson.Stat>();
            var strDate = document.XPathSelectElement("/BkToCstmrStmt/Stmt/FrToDt/FrDtTm")?.Value;
            statement.Date = strDate != null ? DateTime.Parse(strDate) : null;
            foreach (XElement nTry in document.XPathSelectElements("/BkToCstmrStmt/Stmt/Ntry"))
            {


                BsJson.Stat stat = new();
                BsJson.Stat.Rrn rrn = new();
                BsJson.Stat.CPartner cPartner = new();
                BsJson.Stat.CPartner.CAddress cAddress = new();

                rrn.Model = nTry.XPathSelectElement("NtryDtls/TxDtls/Refs/EndToEndId").Value;
                rrn.Number = nTry.XPathSelectElement("NtryDtls/TxDtls/RmtInf/Strd/CdtrRefInf/Ref").Value;


                cPartner.Name = nTry.XPathSelectElement("NtryDtls/TxDtls/RltdPties/Dbtr/Nm")?.Value
                    ?? nTry.XPathSelectElement("NtryDtls/TxDtls/RltdPties/Cdtr/Nm")?.Value;
                cAddress.Street = nTry.XPathSelectElement("NtryDtls/TxDtls/RltdPties/Dbtr/PstlAdr/AdrLine")?.Value
                    ?? nTry.XPathSelectElement("NtryDtls/TxDtls/RltdPties/Cdtr/PstlAdr/AdrLine")?.Value;

                rrn.Model = document.XPathSelectElement("BkToCstmrStmt/Stmt/Ntry").Value;

                stat.Type = "";
                stat.PaymentDescription = "";
                if (nTry.XPathSelectElement("NtryDtls/TxDtls/RmtInf/Strd/AddtlRmtInf") != null)
                {
                    stat.Type = nTry.XPathSelectElement("NtryDtls/TxDtls/RmtInf/Strd/AddtlRmtInf").Value;
                    stat.PaymentDescription = nTry.XPathSelectElement("NtryDtls/TxDtls/RmtInf/Strd/AddtlRmtInf").Value;
                }
                stat.Amount = decimal.Parse(nTry.XPathSelectElement("NtryDtls/TxDtls/AmtDtls/TxAmt/Amt").Value, CultureInfo.InvariantCulture);
                stat.Type = nTry.XPathSelectElement("BkTxCd/Domn/Fmly/Cd").Value.Equals("RCDT") ? "Uplata" : "Isplata";
                stat.RRN = rrn;
                stat.Partner = cPartner;
                stat.Partner.Address = cAddress;
                stat.Number= nTry.XPathSelectElement("AcctSvcrRef").Value;
                statement.Data.Add(stat);
            }

            return statement;
        }
        public override IEnumerable<ValidationResult> Validate(BankStatPart part)
        {
            if (string.IsNullOrEmpty(part.StatementJson))
            {
                yield return new ValidationResult(S["Statement is required."], new[] { nameof(part.StatementJson) });
            }
            var parsed = true;


            try
            {
                ParseStmt(part.StatementJson);
            }
            catch
            {
                parsed = false;
            }




            if (!parsed)
                yield return new ValidationResult(S["Statement not valid"], new[] { nameof(part.StatementJson) });
        }

        public override Task UpdatedAsync(UpdateContentContext context, BankStatPart instance)
        {
            var json = ParseStmt(instance.StatementJson);
            instance.Date.Value = json.Date;
            return Task.CompletedTask;
        }


        public async override Task PublishedAsync(BankStatPart part, PublishContentContext context)
        {
            var json = ParseStmt(part.StatementJson);
            foreach (var pymnt in json.Data)
            {
                var ciPayment = await _session.FirstOrDefaultAsync<PaymentIndex>(_contentManager, x => x.TransactionRef == pymnt.Number);
                if(ciPayment==null)
                    ciPayment = await _contentManager.NewAsync("Payment");
                var payPart = ciPayment.As<Payment>().InitFields();
                payPart.Amount.Value = pymnt.Amount;
                payPart.Address.Text = pymnt.Partner.Address.Street;
                payPart.PayerName.Text = pymnt.Partner.Name;
                payPart.ReferenceNr.Text = pymnt.RRN.Number;
                payPart.BankContentItemId = part.ContentItem.ContentItemId;
                payPart.Description.Text = pymnt.PaymentDescription;
                payPart.IsPayout.Value = pymnt.Type != "Uplata";
                payPart.Date.Value = part.Date.Value;
                payPart.TransactionRef = pymnt.Number;
                if (payPart.IsPayout.Value && payPart.Amount.Value > 0) payPart.Amount.Value = -payPart.Amount.Value; //payouts are negative values
                var version = payPart.IsPayout.Value ? VersionOptions.Published : VersionOptions.Draft;
                if (pymnt.RRN.Number?.Length >= 11)
                {
                    var person = (await _pService.GetByOibAsync(pymnt.RRN.Number?[^11..])).FirstOrDefault();
                    if (person != null)
                    {
                        payPart.Person.ContentItemIds = new[] { person.ContentItemId };
                        version = VersionOptions.Published;
                    }
                }
                ciPayment.Apply(payPart);
                if (ciPayment.CreatedUtc != null)
                    await _contentManager.UpdateAsync(ciPayment);
                else
                    await _contentManager.UpdateValidateAndCreateAsync(ciPayment, version);
            }
        }
    }
}
