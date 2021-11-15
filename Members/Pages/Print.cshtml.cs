using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;

namespace Members.Pages
{
    public class PrintModel : PageModel
    {
        private const string DownloadFormat = "https://altibizhtml2pdf.azurewebsites.net/api/generatePdf?code=bPJSYSigg05QxJvYAsqPr584dwV897UbUialY99kTzNvtEt5lzBSkw==&fileName={0}&url={1}";

        private IContentManager _contentManager;
        private IContentItemDisplayManager _contentItemDisplayManager;
        private IUpdateModelAccessor _updateModelAccessor;

        public IShape PrintHeader { get; set; }
        public IShape Shape { get; set; }

        public PrintModel(IContentItemDisplayManager cidm, IContentManager cm, IUpdateModelAccessor updateModelAccessor)
        {

            _contentManager = cm;
            _contentItemDisplayManager = cidm;
            _updateModelAccessor = updateModelAccessor;
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
            return Redirect(string.Format(DownloadFormat, fileName, docUrl));
        }
    }
}
