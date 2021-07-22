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
        private const string contentType = "Member";
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

        public dynamic ContentItem { get; set; }

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

        public async Task OnGetAsync(string id = contentTypeC)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                NotFound();
            }

            var contentItem = await _contentManager.NewAsync(id);

            var model = await _contentItemDisplayManager.BuildEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);
            ContentItem = model;

        }

        [HttpPost, ActionName("CreateCompany")]
        [FormValueRequired("submit.CreateCompany")]
        public async Task<IActionResult> CreateCompanyPOST([Bind(Prefix = "submit.CreateCompany")] string submitCreateCompany, string returnUrl, string id = contentTypeC)
        {
            var stayOnSamePage = submitCreateCompany == "submit.CreateCompanyAndContinue";

            returnUrl = "/Members/portal";
            // pass a dummy content to the authorization check to check for "own" variations
            var dummyContent = await _contentManager.NewAsync(id);

            return await CreatePOST(id, returnUrl, stayOnSamePage, async contentItem =>
            {
                await _contentManager.PublishAsync(contentItem);

                var typeDefinition = _contentDefinitionManager.GetTypeDefinition(contentItem.ContentType);

                _notifier.Success(string.IsNullOrWhiteSpace(typeDefinition.DisplayName)
                    ? H["Tvoj sadržaj je objavljen."]
                    : H["Tvoj {0}  sadržaj je objavljen.", typeDefinition.DisplayName]);
            });
        }
        private async Task<IActionResult> CreatePOST(string id, string returnUrl, bool stayOnSamePage, Func<ContentItem, Task> conditionallyPublish)
        {
            var contentItem = await _contentManager.NewAsync(id);

            // Set the current user as the owner to check for ownership permissions on creation
            contentItem.Owner = User.Identity.Name;

            var model = await _contentItemDisplayManager.UpdateEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);

            if (!ModelState.IsValid)
            {
                _session.CancelAsync();
                return Page();
            }

            await _contentManager.CreateAsync(contentItem, VersionOptions.Draft);

            await conditionallyPublish(contentItem);

            if ((!string.IsNullOrEmpty(returnUrl)) && (!stayOnSamePage))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToRoute(returnUrl);
        }

    }
}
