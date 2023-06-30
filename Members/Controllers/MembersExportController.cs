using Castle.Core.Internal;
using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using GraphQL;
using GraphQL.Types;
using Members.Base;
using Members.Core;
using Members.Models;
using Members.Persons;
using Members.Utils;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.Users.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UglyToad.PdfPig.AcroForms.Fields;
using YesSql;
using OrchardCore.BackgroundTasks;

namespace Members.Controllers
{
    [Admin]
    public class MembersExportController : Controller
    {
        private readonly MemberService _memberService;
        private ISession _session;
        private readonly IBackgroundTask _backgroundTask;

        public MembersExportController(MemberService mService, ISession session, IBackgroundTask backgroundTask)
        {
            _memberService = mService;
            _backgroundTask = backgroundTask;
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
        public IActionResult DownloadMembersFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var fileName = "Members.csv";

                return File(fileBytes, "application/octet-stream", fileName);
            }

            // Handle the case when the file does not exist
            return NotFound();
        }



        public async Task<FileContentResult> DownloadFileAsync(DateTime date)
        {
            var exportCounty = Request.Form["exportCounty"].ToString();
            var exportActivity = Request.Form["exportActivity"];

            var csvConfig = new CsvConfiguration(new CultureInfo("hr-HR"))
            {
                ShouldQuote = args => true
            };

            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);
            using var csvWriter = new CsvWriter(streamWriter, csvConfig);

            var pageSize = 100; // Adjust the batch size as needed

            await _memberService.WriteMembersToCsv(date, exportCounty, csvWriter, pageSize);
            await _memberService.WriteCompaniesToCsv(date, exportCounty, exportActivity, csvWriter, pageSize);

            await Task.Run(() => csvWriter.Flush());

            memoryStream.Position = 0;

            var bytInStream = memoryStream.ToArray();

            var info = await _session.Query<ExportInfo>().FirstOrDefaultAsync() ?? new ExportInfo();
            info.LastSave = DateTime.Now;
            _session.Save(info);

            return File(bytInStream, "application/octet-stream", "Reports.csv");
        }
        public async Task<IActionResult> TriggerGenerate()
        {
            string allTimeDate = new DateTime(1801, 1, 1).ToString(); // use a format that works for your use case

            var exportCounty = Request.Form["exportCounty"].ToString();
            var exportActivity = Request.Form["exportActivity"];

            // Enqueue the MembersExport background task for execution
            await _backgroundTask.DoWorkAsync(HttpContext.RequestServices, default);

            return RedirectToAction("Index");
        }

    }

    public class Term
    {
        public string Name { get; set; }
        public string termId { get; set; }
    }

}
