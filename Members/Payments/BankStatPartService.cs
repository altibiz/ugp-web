using Members.Base;
using Members.PartFieldSettings;
using Members.Persons;
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
            public int BankID { get; set; }
            public decimal Amount { get; set; }
            public string PaymentDescription { get; set; }
            public string Type { get; set; }
            public bool IsPayout { get; set; }
            public Rrn RRN { get; set; }
            public CPartner Partner { get; set; }
        }

        public List<Stat> Data { get; set; }
    }
    public class BankStatPartService : PartService<BankStatPart>
    {

        public IStringLocalizer<BankStatPartService> S { get; }

        private IContentManager _contentManager;
        private PersonPartService _pService;

        public BankStatPartService(IStringLocalizer<BankStatPartService> S, IContentManager contentManager, PersonPartService personService,IHttpContextAccessor htp):base(htp)
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

        public static BsJson ParseStmt(string xmlstmt)
        {
            try
            {
               return JsonConvert.DeserializeObject<BsJson>(xmlstmt);
            }
            catch
            {

            }
            //if it continues, try xml;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlstmt);

            XmlNode root2 = doc.DocumentElement;
            xmlstmt = Regex.Replace(xmlstmt, "<Document.*?>", "<Document>"); //get rid of namespaces
                                                                                                   //new test
            XElement document = XElement.Parse(xmlstmt);

            //XElement stmt = document.Elements("Document")
            //    .Where(x => (int)x.Element("id") == 20)
            //    .SingleOrDefault();

            BsJson statement = new BsJson();
            statement.Data = new List<BsJson.Stat>();
            
            foreach (XElement nTry in document.XPathSelectElements("/BkToCstmrStmt/Stmt/Ntry"))
            {


                BsJson.Stat stat = new BsJson.Stat();
                BsJson.Stat.Rrn rrn = new BsJson.Stat.Rrn();
                BsJson.Stat.CPartner cPartner = new BsJson.Stat.CPartner();
                BsJson.Stat.CPartner.CAddress cAddress = new BsJson.Stat.CPartner.CAddress();

                rrn.Model = nTry.XPathSelectElement("NtryDtls/TxDtls/Refs/EndToEndId").Value;
                rrn.Number = nTry.XPathSelectElement("NtryDtls/TxDtls/RmtInf/Strd/CdtrRefInf/Ref").Value;


                cPartner.Name = nTry.XPathSelectElement("NtryDtls/TxDtls/RltdPties/Dbtr/Nm").Value;
                cAddress.Street= nTry.XPathSelectElement("NtryDtls/TxDtls/RltdPties/Dbtr/PstlAdr/AdrLine").Value;

                rrn.Model = document.XPathSelectElement("BkToCstmrStmt/Stmt/Ntry").Value;

                stat.Type = nTry.XPathSelectElement("NtryDtls/TxDtls/RmtInf/Strd/AddtlRmtInf").Value;
                stat.PaymentDescription = nTry.XPathSelectElement("NtryDtls/TxDtls/RmtInf/Strd/AddtlRmtInf").Value;
                stat.Amount = decimal.Parse(nTry.XPathSelectElement("NtryDtls/TxDtls/AmtDtls/TxAmt/Amt").Value, CultureInfo.InvariantCulture);
                stat.IsPayout = nTry.XPathSelectElement("BkTxCd/Domn/Fmly/Cd").Value.Equals("ICDT");
                stat.RRN = rrn;
                stat.Partner = cPartner;
                stat.Partner.Address = cAddress;
                statement.Data.Add(stat);

            }

            return statement;

            throw new Exception();


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
                yield return new ValidationResult(S["Statement not a valid json"], new[] { nameof(part.StatementJson) });
        }


        public async override Task PublishedAsync(BankStatPart part, PublishContentContext context)
        {

            var json = ParseStmt(part.StatementJson);
            foreach (var pymnt in json.Data.Where(x => x.Type == "Uplata"))
            {
                var ciPayment = await _contentManager.NewAsync("Payment");
                var payPart = ciPayment.As<Payment>().InitFields();
                payPart.Amount.Value = pymnt.Amount;
                payPart.Address.Text = pymnt.Partner.Address.Street;
                payPart.PayerName.Text = pymnt.Partner.Name;
                payPart.PaymentRef.Text = pymnt.RRN.Number;
                payPart.BankContentItemId = part.ContentItem.ContentItemId;
                payPart.Description.Text = pymnt.PaymentDescription;
                payPart.IsPayout = pymnt.IsPayout;
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
