using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using YesSql;
using OrchardCore;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;

namespace Members.Pages
{
    public class MyCompanyModel : PageModel
    {
        private readonly IContentManager _contentManager;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;

        private readonly IUpdateModelAccessor _updateModelAccessor;

        public dynamic Shape { get; set; }

        public MyCompanyModel(IContentManager contentManager, IContentItemDisplayManager contentItemDisplayManager, IUpdateModelAccessor updateModelAccessor)
        {
            _contentManager = contentManager;
            _contentItemDisplayManager = contentItemDisplayManager;

            _updateModelAccessor = updateModelAccessor;
        }


        public async Task OnGetAsync(string companyId)
        {
            var company = await _contentManager.GetAsync(companyId);

            Shape = await _contentItemDisplayManager.BuildEditorAsync(company, _updateModelAccessor.ModelUpdater, true);

        }
    }
}
