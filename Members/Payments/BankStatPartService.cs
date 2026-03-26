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
using System.Net;
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
        public DateTime? EndDate { get; set; }
        public DateTime? Date { get; set; }
        public string StatementId { get; set; }
        public string LglSeqNbr { get; set; }
    }
    public class BankStatPartService : PartService<BankStatPart>
    {

        public IStringLocalizer<BankStatPartService> S { get; }

        private readonly IContentManager _contentManager;
        private readonly YesSql.ISession _session;

        public BankStatPartService(IStringLocalizer<BankStatPartService> S, IContentManager contentManager, PersonPartService personService, IHttpContextAccessor htp, YesSql.ISession session) : base(htp)
        {
            this.S = S;
            _contentManager = contentManager;
            _session = session;
        }

        public override Action<BankStatPart> GetEditModel(BankStatPart part, BuildPartEditorContext context)
        {
            return m => { m.StatementJson = part.StatementJson; };
        }

        public override Task InitializingAsync(BankStatPart part)
        {
            part.Date.Value = DateTime.Now;
            part.EndDate.Value = DateTime.Now;
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
            //if it continues, try xml or html;

            if (xmlOrJson.Contains("<html", StringComparison.OrdinalIgnoreCase))
                return ParseHtmlStmt(xmlOrJson);

            XmlDocument doc = new();
            doc.LoadXml(xmlOrJson);
            xmlOrJson = Regex.Replace(xmlOrJson, "<Document.*?>", "<Document>"); //get rid of namespaces
                                                                                 //new test
            XElement document = XElement.Parse(xmlOrJson);
            BsJson statement = new()
            {
                Data = []
            };
            foreach (var stmt in document.XPathSelectElements("/BkToCstmrStmt/Stmt"))
            {
                var strDate = stmt.XPathSelectElement("FrToDt/FrDtTm")?.Value;
                statement.Date = statement.Date??(strDate != null ? DateTime.Parse(strDate) : null);
                statement.EndDate = strDate != null ? DateTime.Parse(strDate) : null; //last date in statement is end date;

                var stmtIdElem = stmt.XPathSelectElement("Id");
                statement.StatementId = stmtIdElem != null ? stmtIdElem.Value : null;

                var lglSeqNbrElem = stmt.XPathSelectElement("LglSeqNb");
                statement.LglSeqNbr = lglSeqNbrElem != null ? lglSeqNbrElem.Value : null;
                foreach (XElement nTry in stmt.XPathSelectElements("Ntry"))
                {


                    BsJson.Stat statEntry = new();
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

                    statEntry.PaymentDescription = nTry.XPathSelectElement("NtryDtls/TxDtls/RmtInf/Strd/AddtlRmtInf").Value ?? "";
                    statEntry.Amount = decimal.Parse(nTry.XPathSelectElement("NtryDtls/TxDtls/AmtDtls/TxAmt/Amt").Value, CultureInfo.InvariantCulture);
                    statEntry.Type = nTry.XPathSelectElement("BkTxCd/Domn/Fmly/Cd").Value.Equals("RCDT") ? "Uplata" :
                                nTry.XPathSelectElement("BkTxCd/Domn/Fmly/Cd").Value.Equals("ICDT") ? "Isplata" :
                                throw new NotSupportedException("Payment not supported");
                    statEntry.RRN = rrn;
                    statEntry.Partner = cPartner;
                    statEntry.Partner.Address = cAddress;
                    statEntry.Number = nTry.XPathSelectElement("AcctSvcrRef").Value;
                    statEntry.Date = statement.EndDate;//
                    statement.Data.Add(statEntry);
                }
            }
            return statement;
        }
        private static BsJson ParseHtmlStmt(string html)
        {
            var statement = new BsJson { Data = [] };

            // Extract date from "Za razdoblje" section
            var dateMatch = Regex.Match(html, @"Za razdoblje.*?(\d{2}\.\d{2}\.\d{4})\.", RegexOptions.Singleline);
            if (dateMatch.Success)
            {
                var date = DateTime.ParseExact(dateMatch.Groups[1].Value, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                statement.Date = date;
                statement.EndDate = date;
            }

            // Extract statement number (Broj izvoda) from header section — value is in the last <span>
            var stmtIdMatch = Regex.Match(html, @"Broj izvoda:\s*</span>.*?<span[^>]*>(\d+)</span>", RegexOptions.Singleline);
            if (stmtIdMatch.Success)
            {
                statement.StatementId = stmtIdMatch.Groups[1].Value;
                statement.LglSeqNbr = stmtIdMatch.Groups[1].Value;
            }

            // Extract transaction rows (class="trItems")
            var rowMatches = Regex.Matches(html, @"<tr\s+class=""trItems""[^>]*>(.*?)</tr>", RegexOptions.Singleline);
            foreach (Match rowMatch in rowMatches)
            {
                var tdMatches = Regex.Matches(rowMatch.Groups[1].Value, @"<td[^>]*>(.*?)</td>", RegexOptions.Singleline);
                if (tdMatches.Count < 6) continue;

                var entry = new BsJson.Stat();
                var rrn = new BsJson.Stat.Rrn();
                var partner = new BsJson.Stat.CPartner();
                var address = new BsJson.Stat.CPartner.CAddress();

                // TD 0: dates (valute / obrade)
                var dateText = StripHtml(tdMatches[0].Groups[1].Value);
                var entryDateMatch = Regex.Match(dateText, @"(\d{2}\.\d{2}\.\d{4})");
                if (entryDateMatch.Success)
                    entry.Date = DateTime.ParseExact(entryDateMatch.Groups[1].Value, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                // TD 1: partner name + IBAN
                var partnerLines = StripHtml(tdMatches[1].Groups[1].Value)
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                partner.Name = partnerLines.Length > 0 ? partnerLines[0] : "";
                address.Street = "";
                address.City = "";

                // TD 2: ordinal + description
                var descText = StripHtml(tdMatches[2].Groups[1].Value).Trim();
                var descMatch = Regex.Match(descText, @"^\d+\s*-\s*(.*)$", RegexOptions.Singleline);
                entry.PaymentDescription = descMatch.Success ? descMatch.Groups[1].Value.Trim() : descText;

                // TD 3: references (model / reference number / payment reference)
                var refHtml = Regex.Replace(tdMatches[3].Groups[1].Value, @"<br\s*/?>", "\n");
                var refLines = StripHtml(refHtml)
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                rrn.Model = refLines.Length > 0 ? refLines[0] : "";
                if (refLines.Length > 1)
                {
                    var refLine = refLines[1];
                    // Format "HR00 12-77685993589" — strip the model prefix to get the number
                    var refParts = Regex.Match(refLine, @"^[A-Z]{2}\d{2}\s+(.+)$");
                    rrn.Number = refParts.Success ? refParts.Groups[1].Value : refLine;
                }
                else
                    rrn.Number = "";
                entry.Number = refLines.Length > 2 ? refLines[2] : "";

                // TD 4: Isplata (payout), TD 5: Uplata (payment in)
                var isplataText = StripHtml(tdMatches[4].Groups[1].Value).Trim();
                var uplataText = StripHtml(tdMatches[5].Groups[1].Value).Trim();

                if (!string.IsNullOrEmpty(uplataText) && uplataText != "\u00a0")
                {
                    entry.Type = "Uplata";
                    entry.Amount = ParseCroatianDecimal(uplataText);
                }
                else if (!string.IsNullOrEmpty(isplataText) && isplataText != "\u00a0")
                {
                    entry.Type = "Isplata";
                    entry.Amount = ParseCroatianDecimal(isplataText);
                }

                entry.RRN = rrn;
                partner.Address = address;
                entry.Partner = partner;
                statement.Data.Add(entry);
            }

            return statement;
        }

        private static string StripHtml(string html)
        {
            var text = Regex.Replace(html, @"<br\s*/?>", "\n");
            text = Regex.Replace(text, @"<[^>]+>", "");
            return WebUtility.HtmlDecode(text);
        }

        private static decimal ParseCroatianDecimal(string value)
        {
            // Croatian format: 1.234,56 (dot = thousands separator, comma = decimal)
            value = value.Replace(".", "").Replace(",", ".");
            return decimal.Parse(value, CultureInfo.InvariantCulture);
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
            instance.EndDate.Value = json.EndDate;
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
                payPart.Date.Value = pymnt.Date.Value;
                payPart.TransactionRef = pymnt.Number;
                if (payPart.IsPayout.Value && payPart.Amount.Value > 0) payPart.Amount.Value = -payPart.Amount.Value; //payouts are negative values
                var version = payPart.IsPayout.Value ? VersionOptions.Published : VersionOptions.Draft;
                if (pymnt.RRN.Number?.Length >= 11)
                {
                    var oib = pymnt.RRN.Number[^11..];
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
