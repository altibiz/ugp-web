using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;

namespace Members.Pages
{
    public class PrintModel : PageModel
    {
        //this is a url used for fetching printed doc
        private readonly string downloadFormat;

        private readonly IContentManager _contentManager;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IUpdateModelAccessor _updateModelAccessor;

        public IShape PrintHeader { get; set; }
        public IShape Shape { get; set; }

        public PrintModel(IContentItemDisplayManager cidm, IContentManager cm, IUpdateModelAccessor updateModelAccessor, IConfiguration configuration)
        {

            _contentManager = cm;
            _contentItemDisplayManager = cidm;
            _updateModelAccessor = updateModelAccessor;
            downloadFormat = configuration.GetValue<string>("PrintPdfUrl");
        }

        public async Task<IActionResult> OnGetAsync(string contentId)
        {
            var content = await _contentManager.GetAsync(contentId);
            Shape = await _contentItemDisplayManager.BuildDisplayAsync(content, _updateModelAccessor.ModelUpdater, "Print");
            PrintHeader = await _contentItemDisplayManager.BuildDisplayAsync(content, _updateModelAccessor.ModelUpdater, "PrintHeader");
            return Page();
        }

        public IActionResult OnGetDownload(string contentId, string fileName)
        {
            fileName = string.IsNullOrWhiteSpace(fileName) ? contentId : fileName;
            var docUrl = string.Format("https://{0}/Members/Print/{1}/", Request.Host, contentId);
            return Redirect(string.Format(downloadFormat, fileName, docUrl));
        }
    }
}
