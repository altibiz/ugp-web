using CsvHelper;
using CsvHelper.Configuration;
using Members.Base;
using Members.Core;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.Taxonomies.Fields;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using YesSql;

namespace Members.Controllers
{
    [Admin]
    public class MembersExportController : Controller
    {
        private readonly MemberService _memberService;
        private TaxonomyCachedService _taxService;
        private ISession _session;
        public MembersExportController(TaxonomyCachedService taxService, MemberService mService,ISession session)
        {
            _taxService = taxService;
            _memberService = mService;
            _session = session;
        }
        public async Task<ActionResult> Index()
        {

            var info = await _session.Query<ExportInfo>().FirstOrDefaultAsync() ?? new ExportInfo();
            ViewBag.Date = info.LastSave;
            return View();
        }

        public async Task<FileContentResult> DownloadFileAsync(DateTime date)
        {
            var memList = await _memberService.GetAllMembers(date.Date);

            List<CsvModel> csvList = new List<CsvModel>();

            foreach (var item in memList)
            {
                var member = item.As<Member>();


                var companies = await _memberService.GetMemberCompanies(date.Date, item, true);

                var county = StripCounty(member.ContentItem.Content.PersonPart.County.TagNames[0].ToString());
                var gender  = StripGender(member.ContentItem.Content.Member.Sex.TagNames[0].ToString());
                DateTime birthdate = member.ContentItem.Content.Member.DateOfBirth.Value;
                
                csvList.Add(new CsvModel
                {
                    email = member.ContentItem.Content.PersonPart.Email.Text,
                    ime = member.ContentItem.Content.PersonPart.Name.Text,
                    prezime = member.ContentItem.Content.PersonPart.Surname.Text,

                    tvrtka = "",

                    zemlja = "",

                    datum_rodenja = birthdate.Date.ToString("d.M.yyyy", new CultureInfo("hr-HR")),

                    djelatnost = "",
                    spol = gender,
                    tip_korisnika = "Fizicke",
                    vrsta_organizacije = "",
                    gsm = member.ContentItem.Content.PersonPart.Phone.Text,

                    zupanija = county,
                    mjesto = member.ContentItem.Content.PersonPart.City.Text
                });

                foreach (var company in companies)
                {
                    csvList.Add(await CompanyToCsvModelAsync(company));
                }
            }

            var onlyNewCompanies = await _memberService.GetOnlyNewCompanies(date.Date);

            foreach (var item in onlyNewCompanies)
            {
                csvList.Add(await CompanyToCsvModelAsync(item));
            }

            List<CsvModel> reportCSVModels = csvList;

            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            var csvConfig = new CsvConfiguration(new CultureInfo("hr-HR"))
            {
                ShouldQuote = args => true
            };

            CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfig);

            csvWriter.WriteRecords<CsvModel>(reportCSVModels);
            csvWriter.Flush();
            byte[] bytInStream = memoryStream.ToArray();
            memoryStream.Close();

            var info = await _session.Query<ExportInfo>().FirstOrDefaultAsync() ?? new ExportInfo();
            info.LastSave=DateTime.Now;
            _session.Save(info);
            return File(bytInStream, "application/octet-stream", "Reports.csv");
        }

        public string StripCounty(string county)
        {
            string str = county.ToUpper();
                str = str.Replace("ŽUPANIJA", "");
                str = str.Replace("Ž", "Z");
                str = str.Replace("Ć", "C");
                str = str.Replace("Č", "C");
                str = str.Replace("Đ", "D");
                str = str.Replace("Š", "S");
            str = str.Trim();
            return str;
        }
        public string StripGender(string county)
        {
            string str = county.ToUpper();
            str = str.Replace("MUŠKO", "M");
            str = str.Replace("ŽENSKO", "F");
            str = str.Trim();
            return str;
        }

        public async Task<CsvModel> CompanyToCsvModelAsync(ContentItem company, ContentItem member = null)
        {

            if (member==null)
            {
                member = await _memberService.GetCompanyMember(company);
            }

            DateTime birthdate = member.ContentItem.Content.Member.DateOfBirth.Value;

            var gender = StripGender(member.ContentItem.Content.Member.Sex.TagNames[0].ToString());

            var cCounty = StripCounty(company.ContentItem.Content.PersonPart.County.TagNames[0].ToString());

            return new CsvModel
            {
                email = company.ContentItem.Content.PersonPart.Email.Text,
                ime = member.ContentItem.Content.PersonPart.Name.Text,
                prezime = member.ContentItem.Content.PersonPart.Surname.Text,

                tvrtka = company.ContentItem.Content.PersonPart.Name.Text,

                zemlja = "",

                datum_rodenja =birthdate.Date.ToString("d.M.yyyy", new CultureInfo("hr-HR")),

                djelatnost = string.Join(", ", company.ContentItem.Content.Company.Activity.TagNames),
                spol = gender,
                tip_korisnika = "Pravne",
                vrsta_organizacije = company.ContentItem.Content.Company.OrganisationType.TagNames[0],
                gsm = company.ContentItem.Content.PersonPart.Phone.Text,
                zupanija = cCounty,
                mjesto = company.ContentItem.Content.PersonPart.City.Text
            };
        }

    }

    public class ExportInfo
    {
        public DateTime? LastSave { get; set; }
    }

    public class CsvModel
    {
        public string email { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public string tvrtka { get; set; }
        public string zemlja { get; set; }
        public string datum_rodenja { get; set; }
        public string djelatnost { get; set; }
        public string spol { get; set; }
        public string tip_korisnika { get; set; }
        public string vrsta_organizacije { get; set; }
        public string gsm { get; set; }
        public string pretplacen { get; set; }
        public string zupanija { get; set; }
        public string mjesto { get; set; }
    }
}
