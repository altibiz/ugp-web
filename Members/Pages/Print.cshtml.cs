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
        private IContentManager _contentManager;
        private IContentItemDisplayManager _contentItemDisplayManager;
        private IUpdateModelAccessor _updateModelAccessor;

        public IShape PrintHeader { get; set; }
        public IShape Shape { get; set; }

        public PrintModel(IContentItemDisplayManager cidm,IContentManager cm,IUpdateModelAccessor updateModelAccessor)
        {

            _contentManager = cm;
            _contentItemDisplayManager = cidm;
            _updateModelAccessor = updateModelAccessor;
        }

        public async Task<IActionResult> OnGetAsync(string contentId)
        {
            var content = await _contentManager.GetAsync(contentId);
            Shape =await _contentItemDisplayManager.BuildDisplayAsync(content, _updateModelAccessor.ModelUpdater, "Print");
            PrintHeader = await _contentItemDisplayManager.BuildDisplayAsync(content, _updateModelAccessor.ModelUpdater, "PrintHeader");
            return Page();
        }
    }
}
