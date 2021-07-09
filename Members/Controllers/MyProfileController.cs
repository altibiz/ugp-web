using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.Controllers
{
    public class MyProfileController : Controller
    {
        private const string contentType = "Member";
        private const string contentTypeC = "Company";

        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IHtmlLocalizer H;
        private readonly dynamic New;
        private readonly INotifier _notifier;
        private readonly ISession _session;
        private readonly ISiteService _siteService;
        private readonly IUpdateModelAccessor _updateModelAccessor;


        public MyProfileController(IContentManager contentManager, IContentDefinitionManager contentDefinitionManager, IContentItemDisplayManager contentItemDisplayManager, IHtmlLocalizer<HomeController> htmlLocalizer, INotifier notifier, ISession session, IShapeFactory shapeFactory, ISiteService siteService, IUpdateModelAccessor updateModelAccessor)
        {
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _contentItemDisplayManager = contentItemDisplayManager;
            _notifier = notifier;
            _session = session;
            _siteService = siteService;
            _updateModelAccessor = updateModelAccessor;

            H = htmlLocalizer;
            New = shapeFactory;
        }

        public async Task<IActionResult> IndexAsync(string id = contentType)
        {

            if (String.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var contentItem = await _contentManager.NewAsync(id);


            var model = await _contentItemDisplayManager.BuildEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);

            return View(model);

            return View();
        }
    }
}