using Members.Base;
using Members.PartFieldSettings;
using Members.Persons;
using Members.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Handlers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.Json;
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
        public string StatementId { get; set; }
        public string LglSeqNbr { get; set; }
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
            part.Date.Value = DateTime.Now;
            return Task.CompletedTask;
        }

        public static BsJson ParseStmt(string xmlOrJson)
        {
            try
            {
                var res = JsonSerializer.Deserialize<BsJson>(xmlOrJson);
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

            var stmtIdElem = document.XPathSelectElement("/BkToCstmrStmt/Stmt/Id");
            statement.StatementId = stmtIdElem != null ? stmtIdElem.Value : null;

            var lglSeqNbrElem = document.XPathSelectElement("/BkToCstmrStmt/Stmt/LglSeqNb");
            statement.LglSeqNbr = lglSeqNbrElem != null ? lglSeqNbrElem.Value : null;
            foreach (XElement nTry in document.XPathSelectElements("/BkToCstmrStmt/Stmt/Ntry"))
            {


                BsJson.Stat stat = new();
                BsJson.Stat.Rrn rrn = new();
                BsJson.Stat.CPartner cPartner = new();
                BsJson.Stat.CPartner.CAddress cAddress = new();

                rrn.Model = nTry.XPathSelectElement("NtryDtls/TxDtls/Refs/EndToEndId").Value;
                rrn.Number = nTry.XPathSelectElement("NtryDtls/TxDtls/RmtInf/Strd/CdtrRefInf/Ref").Value;

                var party = nTry.XPathSelectElement("NtryDtls/TxDtls/RltdPties/Dbtr")
                            ?? nTry.XPathSelectElement("NtryDtls/TxDtls/RltdPties/Cdtr");
                cPartner.Name =
                       party.XPathSelectElement("Nm")?.Value
                    ?? party.XPathSelectElement("Pty/Nm")?.Value;
                cAddress.Street =
                       party.XPathSelectElement("PstlAdr/AdrLine")?.Value
                    ?? party.XPathSelectElement("Pty/PstlAdr/AdrLine")?.Value;

                cAddress.City =
                       party.XPathSelectElement("PstlAdr/TownNm")?.Value
                    ?? party.XPathSelectElement("Pty/PstlAdr/TownNm")?.Value;

                stat.PaymentDescription = nTry.XPathSelectElement("NtryDtls/TxDtls/RmtInf/Strd/AddtlRmtInf").Value ?? "";
                stat.Amount = decimal.Parse(nTry.XPathSelectElement("NtryDtls/TxDtls/AmtDtls/TxAmt/Amt").Value, CultureInfo.InvariantCulture);
                stat.Type = nTry.XPathSelectElement("BkTxCd/Domn/Fmly/Cd").Value.Equals("RCDT") ? "Uplata" :
                            nTry.XPathSelectElement("BkTxCd/Domn/Fmly/Cd").Value.Equals("ICDT") ? "Isplata" :
                            throw new NotSupportedException("Payment not supported");
                stat.RRN = rrn;
                stat.Partner = cPartner;
                stat.Partner.Address = cAddress;
                stat.Number = nTry.XPathSelectElement("AcctSvcrRef").Value;
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
            instance.StatementId.Text = json.StatementId;
            instance.SequenceId = json.LglSeqNbr;
            return Task.CompletedTask;
        }


        public async override Task PublishedAsync(BankStatPart part, PublishContentContext context)
        {
            var json = ParseStmt(part.StatementJson);
            foreach (var pymnt in json.Data)
            {
                var ciPayment = await _session.FirstOrDefaultAsync<PaymentIndex>(_contentManager, x => x.TransactionRef == pymnt.Number);
                if (ciPayment == null)
                    ciPayment = await _contentManager.NewAsync("Payment");
                var payPart = ciPayment.AsInit<Payment>();
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
                    var oib= pymnt.RRN.Number[^11..];
                    payPart.PayerOib.Text = oib;
                    var person = await _session.GetByOib(oib);
                    if (person != null)
                    {
                        payPart.Person.ContentItemIds = [person.ContentItemId];
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
