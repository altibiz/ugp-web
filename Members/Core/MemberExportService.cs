using CsvHelper.Configuration;
using CsvHelper;
using Members.Persons;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YesSql;
using Members.Base;
using ISession = YesSql.ISession;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.BackgroundTasks;
using System.Collections.Concurrent;
using System.Threading;
using OrchardCore.Email;
using Castle.Core.Internal;
using OrchardCore.ContentManagement.Records;
using Castle.Core.Logging;

namespace Members.Core
{
    public class MemberExportService
    {
        private readonly MemberService _memberService;
        readonly IHttpContextAccessor httpContextAccessor;
        private readonly ISession _session;
        private readonly ILogger _logger;

        public MemberExportService(MemberService memberService, IHttpContextAccessor httpContextAccessor, ISession session, ILogger logger)
        {
            _memberService = memberService;
            this.httpContextAccessor = httpContextAccessor;
            _session = session;
            _logger = logger;
        }

        public string StripCounty(string county)
        {
            if (string.IsNullOrEmpty(county)) return "";
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
            if (string.IsNullOrEmpty(county)) return "";
            string str = county.ToUpper();
            str = str.Replace("MUŠKO", "M");
            str = str.Replace("ŽENSKO", "F");
            str = str.Trim();
            return str;
        }
        public IQuery<ContentItem> GetAllMembersForExportQuery(ExportModel model)
        {
            if (!model.ExportActivity.Any(string.IsNullOrEmpty))
                return _session.Query<ContentItem, ContentItemIndex>(x => x.ContentItemId == "false_dummy");

            IQuery<ContentItem> query = _session.Query<ContentItem, ContentItemIndex>(x => x.ContentType == nameof(Member) && x.Published && x.Latest).OrderBy(x => x.ContentItemId);
            if (model.Date < DateTime.Now.Date) query = query.With<ContentItemIndex>(x => x.PublishedUtc >= model.Date);

            if (!string.IsNullOrEmpty(model.ExportCounty))
                query = query.GetByTerm(nameof(PersonPart), nameof(PersonPart.County), model.ExportCounty);

            return query;
        }

        public IQuery<ContentItem> GetAllCompaniesForExportQuery(ExportModel model)
        {
            IQuery<ContentItem> query = _session.Query<ContentItem>();
            query = query.With<ContentItemIndex>(x => x.ContentType == nameof(Company) && x.Published && x.Latest);
            if (model.Date < DateTime.Now.Date) query = query.With<ContentItemIndex>().Where(x => x.PublishedUtc > model.Date);

            if (!string.IsNullOrEmpty(model.ExportCounty))
                query = query.GetByTerm(nameof(PersonPart), nameof(PersonPart.County), model.ExportCounty);

            if (!model.ExportActivity.Any(string.IsNullOrEmpty))
                query = query.GetByTerm(nameof(Company), nameof(Company.Activity), model.ExportActivity);

            return query;
        }

        public async Task<Stream> GetExportFile(ExportModel model)
        {
            Dictionary<string, CsvModel> csvList = new();
            var count = await GetAllMembersForExportQuery(model).CountAsync();
            var take = 500;
            Dictionary<string, (Member, PersonPart)> members = new();
            for (var skip = 0; skip < count; skip += take)
            {
                var list = await GetAllMembersForExportQuery(model).Take(take).Skip(skip).ListAsync();
                foreach (var itm in list)
                {
                    members[itm.ContentItemId] = (itm.As<Member>(), itm.As<PersonPart>());
                }
            }

            foreach (var item in members)
            {
                var member = item.Value.Item1;
                var person = item.Value.Item2;

                var county = StripCounty((await person.County.GetTerm(httpContextAccessor.HttpContext))?.DisplayText ?? "");
                var gender = StripGender((await member.Sex.GetTerm(httpContextAccessor.HttpContext))?.DisplayText ?? "");
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

            count = await GetAllCompaniesForExportQuery(model).CountAsync();
            for (var skip = 0; skip < count; skip += take)
            {
                var list = await GetAllCompaniesForExportQuery(model).Take(take).Skip(skip).ListAsync();
                foreach (var item in list)
                {
                    var parentTuple = item.GetParentItemId() != null && members.TryGetValue(item.GetParentItemId(), out var member) ? member : new();
                    var csv = await CompanyToCsvModelAsync(item, parentTuple);
                    if (csv == null || string.IsNullOrEmpty(csv.email)) continue;
                    csvList[csv.email] = csv;
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
            var info = await _session.Query<ExportInfo>().FirstOrDefaultAsync() ?? new ExportInfo();
            info.LastSave = DateTime.Now;
            await _session.SaveAsync(info);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public async Task<CsvModel> CompanyToCsvModelAsync(ContentItem company, (Member, PersonPart) parentMember)
        {
            try
            {
                if (parentMember.Item1 == null)
                {
                    var member = await _memberService.GetCompanyMember(company);
                    parentMember.Item1 = member?.As<Member>();
                    parentMember.Item2 = member?.As<PersonPart>();
                }

                var mpart = parentMember.Item1;
                var ppart = parentMember.Item2;
                var cperpart = company.As<PersonPart>();
                var compart = company.As<Company>();

                DateTime? birthdate = mpart?.DateOfBirth?.Value;

                var county = StripCounty((await cperpart?.County.GetTerm(httpContextAccessor.HttpContext))?.DisplayText);
                var gender = StripGender(mpart != null ? (await mpart?.Sex.GetTerm(httpContextAccessor.HttpContext))?.DisplayText : null);

                var activityTerms = await compart.Activity?.GetTerms(httpContextAccessor.HttpContext);

                CsvModel cs = new CsvModel();

                cs.email = cperpart.Email?.Text;
                cs.ime = ppart?.Name?.Text ?? cperpart?.Name?.Text;
                cs.prezime = ppart?.Surname?.Text ?? cperpart?.Surname?.Text; ;
                cs.tvrtka = cperpart.Name?.Text;
                cs.datum_rodjenja = birthdate.HasValue ? birthdate.Value.Date.ToString("yyyy-MM-dd", new CultureInfo("hr-HR")) : "";
                cs.djelatnost = string.Join(", ", activityTerms?.Select(x => x?.DisplayText));
                cs.spol = gender;
                cs.tip_korisnika = "Pravne";
                cs.gsm = cperpart.Phone?.Text;
                cs.zupanija = county;
                cs.mjesto = cperpart.City?.Text;
                cs.oib = cperpart.Oib?.Text;
                return cs;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error while converting company to csv model id: {company?.ContentItemId}, {company?.DisplayText}", ex);
                return null;
            }
        }
    }

    public class ExportModel
    {
        public DateTime Date { get; set; }
        public string ExportCounty { get; set; }
        public string[] ExportActivity { get; set; }
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
        public string oib { get; set; }
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


    [BackgroundTask(Schedule = "*/1 * * * *", Description = "Fast import background task.")]
    public class MemberExportBackgroundTask : IBackgroundTask
    {
        public static readonly ConcurrentQueue<(ExportModel, string)> PendingImports = new();
        public async Task DoWorkAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            if (PendingImports.TryDequeue(out var toImport))
            {
                var service = serviceProvider.GetRequiredService<MemberExportService>();
                var file = await service.GetExportFile(toImport.Item1);
                if (file != null)
                {
                    var _emailService = serviceProvider.GetRequiredService<IEmailService>();
                    var msg = new MailMessage
                    {
                        From = toImport.Item2,
                        To = toImport.Item2,
                        Subject = "Your file export",
                    };
                    msg.Attachments.Add(new MailMessageAttachment
                    {
                        Filename = "members.csv",
                        Stream = file,
                    });
                    await _emailService.SendAsync(msg);
                }
            }
        }
    }
}
