using Dapper;
using Members.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Admin;
using OrchardCore.DisplayManagement.Notify;
using System.Collections.Generic;
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
        private INotifier _notifier;

        public IHtmlLocalizer<MembersExportController> H { get; }

        private MemberExportService _exportService;

        public MembersExportController(MemberService mService, ISession session, INotifier notifier, IHtmlLocalizer<MembersExportController> htmlLocalizer,
            MemberExportService exportService)
        {
            _memberService = mService;
            _session = session;
            _notifier = notifier;
            H = htmlLocalizer;
            _exportService = exportService;
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
            var ciList = allTerms.Select(x => new Term { Name = x.DisplayText, termId = x.ContentItemId });
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

        public async Task<ActionResult> DownloadFileAsync(ExportModel model)
        {
            var count = await _exportService.GetAllMembersForExportQuery(model).CountAsync();
            var countCompany = await _exportService.GetAllCompaniesForExportQuery(model).CountAsync();
            if (countCompany + count < 1000)
            {
                var stream = await _exportService.GetExportFile(model);
                return File(stream, "application/octet-stream", "Reports.csv");
            }
            var user = await _memberService.GetCurrentUser();
            await _notifier.WarningAsync(H["File too big to export there are " + (count + countCompany) + " items in the list. It will be sent to: " + user.Email]);
            MemberExportBackgroundTask.PendingExports.Enqueue((model, user.Email));
            return RedirectToAction("Index");
        }
    }
}
