using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Routing;
using OrchardCore.Users.Services;
using YesSql;
namespace Members.Pages
{
    public class CreateCompanyModel : PageModel
    {
        private const string contentTypeC = "Company";

        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IHtmlLocalizer H;
        private readonly dynamic New;
        private readonly INotifier _notifier;
        private readonly ISession _session;
        private readonly IUpdateModelAccessor _updateModelAccessor;
        private readonly IUserService _userService;

        public dynamic Shape { get; set; }

        public CreateCompanyModel(IContentManager contentManager, IUserService userService, IContentDefinitionManager contentDefinitionManager, IContentItemDisplayManager contentItemDisplayManager, IHtmlLocalizer<CreateMemberModel> htmlLocalizer, INotifier notifier, ISession session, IUpdateModelAccessor updateModelAccessor)
        {
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _contentItemDisplayManager = contentItemDisplayManager;
            _notifier = notifier;
            _session = session;
            _updateModelAccessor = updateModelAccessor;
            _userService = userService;

            H = htmlLocalizer;
        }

        public async Task OnGetAsync()
        {

            var contentItem = await _contentManager.NewAsync(contentTypeC);

            var model = await _contentItemDisplayManager.BuildEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);
            Shape = model;

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var contentItem = await _contentManager.NewAsync(contentTypeC);

            // Set the current user as the owner to check for ownership permissions on creation
            contentItem.Owner = User.Identity.Name;

            var model = await _contentItemDisplayManager.UpdateEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);

            if (!ModelState.IsValid)
            {
                await _session.CancelAsync();

                Shape = model;
                return Page();
            }

            await _contentManager.CreateAsync(contentItem, VersionOptions.Draft);

            await _contentManager.PublishAsync(contentItem);

            _notifier.Success( H["Legal entity added successfully"]);

            return RedirectToRoute("Portal");
        }

    }
}
