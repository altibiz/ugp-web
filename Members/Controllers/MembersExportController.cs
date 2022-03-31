using CsvHelper;
using CsvHelper.Configuration;
using Members.Base;
using Members.Core;
using Members.Persons;
using Members.Utils;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YesSql;

namespace Members.Controllers
{
    [Admin]
    public class MembersExportController : Controller
    {
        private readonly MemberService _memberService;
        private ISession _session;
        public MembersExportController(MemberService mService, ISession session)
        {
            _memberService = mService;
            _session = session;
        }
        public async Task<ActionResult> Index()
        {

            var info = await _session.Query<ExportInfo>().FirstOrDefaultAsync() ?? new ExportInfo();
            return View(info);
        }

        public async Task<FileContentResult> DownloadFileAsync(DateTime date)
        {
            var memList = await _memberService.GetAllMembers(date.Date);

            Dictionary<string,CsvModel> csvList = new();

            foreach (var item in memList)
            {
                var member = item.As<Member>();
                var person = item.As<PersonPart>();

                var county = StripCounty((await person.County.GetTerm(HttpContext))?.DisplayText ?? "");
                var gender = StripGender((await member.Sex.GetTerm(HttpContext))?.DisplayText ?? "");
                DateTime? birthdate = member.DateOfBirth?.Value;
                var memberCsv = new CsvModel
                {
                    email = person.Email?.Text,
                    ime = person.Name?.Text,
                    prezime = person.Surname?.Text,

                    tvrtka = "",


                    datum_rodjenja = birthdate.HasValue ? birthdate.Value.ToString("yyyy-MM-dd", new CultureInfo("hr-HR")) : "",

                    djelatnost = "",
                    spol = gender,
                    tip_korisnika = "Fizičke",
                    gsm = person.Phone?.Text,

                    zupanija = county,
                    mjesto = person.City?.Text
                };
                if (string.IsNullOrEmpty(memberCsv.email)) continue;
                csvList[memberCsv.email] = memberCsv;
            }

            var onlyNewCompanies = await _memberService.GetOnlyNewCompanies(date.Date);

            foreach (var item in onlyNewCompanies)
            {
                var csv = await CompanyToCsvModelAsync(item);
                if (string.IsNullOrEmpty(csv.email)) continue;
                csvList[csv.email] = csv;
            }

            List<CsvModel> reportCSVModels = csvList.Values.ToList();

            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            var csvConfig = new CsvConfiguration(new CultureInfo("hr-HR"))
            {
                ShouldQuote = args => true
            };

            CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfig);

            csvWriter.WriteRecords(reportCSVModels);
            csvWriter.Flush();
            byte[] bytInStream = memoryStream.ToArray();
            memoryStream.Close();

            var info = await _session.Query<ExportInfo>().FirstOrDefaultAsync() ?? new ExportInfo();
            info.LastSave = DateTime.Now;
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

            if (member == null)
            {
                member = await _memberService.GetCompanyMember(company);
            }

            var mpart = member.As<Member>();
            var ppart = member.As<PersonPart>();
            var cppart = company.As<PersonPart>();
            var compart=company.As<Company>();

            DateTime? birthdate = mpart.DateOfBirth.Value;

            var county = StripCounty((await ppart.County.GetTerm(HttpContext))?.DisplayText ?? "");
            var gender = StripGender((await mpart.Sex.GetTerm(HttpContext))?.DisplayText ?? "");

            return new CsvModel
            {
                email = cppart.Email?.Text,
                ime = ppart.Name?.Text,
                prezime = ppart.Surname?.Text,

                tvrtka = cppart.Name?.Text,


                datum_rodjenja = birthdate.HasValue ? birthdate.Value.Date.ToString("yyyy-MM-dd", new CultureInfo("hr-HR")) : "",

                djelatnost = string.Join(", ", (await compart.Activity.GetTerms(HttpContext)).Select(x=>x.DisplayText)),
                spol = gender,
                tip_korisnika = "Pravne",
                gsm = cppart.Phone?.Text,
                zupanija = county,
                mjesto = cppart.City?.Text
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
        public string datum_rodjenja { get; set; }
        public string djelatnost { get; set; }
        public string spol { get; set; }
        public string tip_korisnika { get; set; }
        public string gsm { get; set; }
        public string pretplacen { get; set; }
        public string zupanija { get; set; }
        public string mjesto { get; set; }
    }
}
