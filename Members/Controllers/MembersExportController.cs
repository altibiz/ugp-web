using Castle.Core.Internal;
using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using GraphQL;
using GraphQL.Types;
using Members.Base;
using Members.Core;
using Members.Persons;
using Members.Utils;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UglyToad.PdfPig.AcroForms.Fields;
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
            info.CountyList = await GetCountiesAsync();
            info.ActivityList = await GetActivitiesAsync();
            return View(info);
        }

        private async Task<List<Term>> GetCountiesAsync()
        {
            var allTerms = await _memberService.GetTaxonomyTerms("Županija");
            var ciList =  allTerms.Select(x => new Term { Name= x.DisplayText ,termId=x.ContentItemId });
            ciList = ciList.Prepend(new Term { Name = "Sve županije", termId = null });
            
            return ciList.ToList();
        }

        private async Task<List<Term>> GetActivitiesAsync()
        {
            var allTerms = await _memberService.GetTaxonomyTerms("Djelatnost");
            var ciList = allTerms.Select(x => new Term { Name = x.DisplayText, termId = x.ContentItemId });
            ciList = ciList.Prepend(new Term { Name = "Sve djelatnosti", termId = null });

            return ciList.ToList();
        }
        
        public async Task<FileContentResult> DownloadFileAsync(DateTime date)
        {
            //get exportCounty selected from UI
            var exportCounty = Request.Form["exportCounty"].ToString();
            var exportActivity = Request.Form["exportActivity"];

            Dictionary<string, CsvModel> csvList = new();

            DateTime startDate = date;

            if (startDate <= DateTime.Now)
            {
                IEnumerable<ContentItem> memList = new List<ContentItem>();

                int pageNum = 1;
                int pageSize = 50; //change this as per your requirement
                bool hasMoreRecords = true;

                while (hasMoreRecords)
                {
                    memList = await _memberService.GetAllMembersForExport(startDate, exportCounty, pageNum, pageSize);

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
                            mjesto = person.City?.Text,
                            oib = person.Oib?.Text
                        };
                        if (string.IsNullOrEmpty(memberCsv.email)) continue;
                        csvList[memberCsv.email] = memberCsv;
                    }
                    if (memList.Count() < pageSize)
                    {
                        hasMoreRecords = false;
                    }
                    else
                    {
                        pageNum++;
                    }
                }
                IEnumerable<ContentItem> onlyNewCompanies = new List<ContentItem>();
                pageNum = 1;
                hasMoreRecords = true;
                while (hasMoreRecords)
                {
                    onlyNewCompanies = await _memberService.GetAllCompaniesForExport(startDate, exportCounty, exportActivity, pageNum, pageSize);

                    foreach (var item in onlyNewCompanies)
                    {
                        var csv = await CompanyToCsvModelAsync(item);
                        if (csv == null || string.IsNullOrEmpty(csv.email)) continue;
                        csvList[csv.email] = csv;
                    }
                    if (onlyNewCompanies.Count() < pageSize)
                    {
                        hasMoreRecords = false;
                    }
                    else
                    {
                        pageNum++;
                    }
                }
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
                if (member == null) return null;
            }

            var mpart = member.As<Member>();
            var ppart = member.As<PersonPart>();
            var cppart = company.As<PersonPart>();
            var compart=company.As<Company>();

            DateTime? birthdate = mpart?.DateOfBirth?.Value;

            var county = StripCounty((await cppart.County.GetTerm(HttpContext))?.DisplayText ?? "");
            var gender = StripGender((await mpart.Sex.GetTerm(HttpContext))?.DisplayText ?? "");

            var activityTerms = await compart.Activity?.GetTerms(HttpContext);

            CsvModel cs = new CsvModel();

            cs.email = cppart.Email?.Text;
            cs.ime = ppart.Name?.Text;
            cs.prezime = ppart.Surname?.Text;
            cs.tvrtka = cppart.Name?.Text;
            cs.datum_rodjenja = birthdate.HasValue ? birthdate.Value.Date.ToString("yyyy-MM-dd", new CultureInfo("hr-HR")) : "";
            cs.djelatnost = string.Join(", ", activityTerms?.Select(x => x?.DisplayText));
            cs.spol = gender;
            cs.tip_korisnika = "Pravne";
            cs.gsm = cppart.Phone?.Text;
            cs.zupanija = county;
            cs.mjesto = cppart.City?.Text;
            cs.oib = cppart.Oib?.Text;
            return cs;
        }
    }
    
    public class ExportInfo
    {
        public DateTime? LastSave { get; set; }
        public List<Term> CountyList { get; set; }
        public List<Term> ActivityList { get; set; }
    }

    public class Term
    {
        public string Name { get; set; }
        public string termId { get; set; }
    }
    
    public class CsvModel
    {
        public string email { get; set; }
        public string ime { get; set;  }
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
        public string oib { get; set; }
    }
}
