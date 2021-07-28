using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Routing;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Records;
using Members.ViewModels;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Navigation;
using OrchardCore.Routing;
using OrchardCore.Settings;
using YesSql;
using OrchardCore.Users.Services;
using Microsoft.AspNetCore.Authorization;

namespace Members.Controllers
{
    [Authorize]
    public class HomeController : Controller
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
        private readonly IUserService _userService;

        public HomeController(IContentManager contentManager, IUserService userService, IContentDefinitionManager contentDefinitionManager, IContentItemDisplayManager contentItemDisplayManager, IHtmlLocalizer<HomeController> htmlLocalizer, INotifier notifier, ISession session, IShapeFactory shapeFactory, ISiteService siteService, IUpdateModelAccessor updateModelAccessor)
        {
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _contentItemDisplayManager = contentItemDisplayManager;
            _notifier = notifier;
            _session = session;
            _siteService = siteService;
            _updateModelAccessor = updateModelAccessor;
            _userService = userService;

            H = htmlLocalizer;
            New = shapeFactory;
        }
        public async Task<IActionResult> Create(string id = contentType)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var contentItem = await _contentManager.NewAsync(id);


            var model = await _contentItemDisplayManager.BuildEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);


            _notifier.Success(H["Korisnik napravljen, dodaj èlana."]);

            return View(model);
        }
        public async Task<IActionResult> CreateCompany(string id = contentTypeC)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var contentItem = await _contentManager.NewAsync(id);


            var model = await _contentItemDisplayManager.BuildEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);

            return View(model);
        }

        [HttpPost, ActionName("Create")]
        [FormValueRequired("submit.Create")]
        public async Task<IActionResult> CreatePOST([Bind(Prefix = "submit.Create")] string submitCreate, string returnUrl, string id = contentType)
        {
            var user = await _userService.GetAuthenticatedUserAsync(User) as OrchardCore.Users.Models.User;
            returnUrl = "/Members/portal";

            var stayOnSamePage = submitCreate == "submit.CreateAndContinue";
            // pass a dummy content to the authorization check to check for "own" variations
            var dummyContent = await _contentManager.NewAsync(id);

            return await CreatePOST(id, returnUrl, stayOnSamePage, async contentItem =>
            {

                contentItem.Content.Member.User.UserIds.Add(user.UserId);
                contentItem.Content.Member.User.UserNames.Add(user.UserName);

                await _contentManager.PublishAsync(contentItem);

                var typeDefinition = _contentDefinitionManager.GetTypeDefinition(contentItem.ContentType);

                _notifier.Success(string.IsNullOrWhiteSpace(typeDefinition.DisplayName)
                    ? H["Tvoj sadržaj je objavljen."]
                    : H["Tvoj {0} je objavljen.", typeDefinition.DisplayName]);
            });
        }

        [HttpPost, ActionName("Create")]
        [FormValueRequired("submit.CreateToCompany")]
        public async Task<IActionResult> CreateToCompanyPOST([Bind(Prefix = "submit.CreateToCompany")] string submitCreate, string returnUrl, string id = contentType)
        {
            var stayOnSamePage = submitCreate == "submit.CreateAndContinue";

            returnUrl = "/Members/Home/CreateCompany";
            // pass a dummy content to the authorization check to check for "own" variations
            var dummyContent = await _contentManager.NewAsync(id);

            return await CreatePOST(id, returnUrl, stayOnSamePage, async contentItem =>
            {

                await _contentManager.PublishAsync(contentItem);

                var typeDefinition = _contentDefinitionManager.GetTypeDefinition(contentItem.ContentType);

                _notifier.Success(string.IsNullOrWhiteSpace(typeDefinition.DisplayName)
                    ? H["Tvoj sadržaj je objavljen."]
                    : H["Tvoj {0} je objavljen.", typeDefinition.DisplayName]);
            });
        }

        [HttpPost, ActionName("CreateCompany")]
        [FormValueRequired("submit.Create")]
        public async Task<IActionResult> CreateCompanyPOST([Bind(Prefix = "submit.Create")] string submitCreate, string returnUrl, string id = contentTypeC)
        {
            var stayOnSamePage = submitCreate == "submit.CreateAndContinue";

            returnUrl = "/Members/portal";
            // pass a dummy content to the authorization check to check for "own" variations
            var dummyContent = await _contentManager.NewAsync(id);

            return await CreatePOST(id, returnUrl, stayOnSamePage, async contentItem =>
            {
                await _contentManager.PublishAsync(contentItem);

                var typeDefinition = _contentDefinitionManager.GetTypeDefinition(contentItem.ContentType);

                _notifier.Success(string.IsNullOrWhiteSpace(typeDefinition.DisplayName)
                    ? H["Tvoj sadržaj je objavljen."]
                    : H["Tvoj {0} je objavljen.", typeDefinition.DisplayName]);
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
                return View(model);
            }

            await _contentManager.CreateAsync(contentItem, VersionOptions.Draft);

            await conditionallyPublish(contentItem);

            if ((!string.IsNullOrEmpty(returnUrl)) && (!stayOnSamePage))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToRoute(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string contentItemId)
        {
            var contentItem = await _contentManager.GetAsync(contentItemId, VersionOptions.Latest);

            if (contentItem == null)
                return NotFound();

            var model = await _contentItemDisplayManager.BuildEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, false);
            _notifier.Success(H["Blje"]);
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("submit.Publish")]
        public async Task<IActionResult> EditAndPublishPOST(string contentItemId, [Bind(Prefix = "submit.Publish")] string submitPublish, string returnUrl)
        {
            var stayOnSamePage = submitPublish == "submit.PublishAndContinue";

            var content = await _contentManager.GetAsync(contentItemId, VersionOptions.Latest);

            if (content == null)
            {
                return NotFound();
            }

            return await EditPOST(contentItemId, returnUrl, stayOnSamePage, async contentItem =>
            {
                await _contentManager.PublishAsync(contentItem);

                var typeDefinition = _contentDefinitionManager.GetTypeDefinition(contentItem.ContentType);

                _notifier.Success(string.IsNullOrWhiteSpace(typeDefinition.DisplayName)
                    ? H["Your content has been published."]
                    : H["Your {0} has been published.", typeDefinition.DisplayName]);
            });
        }

        private async Task<IActionResult> EditPOST(string contentItemId, string returnUrl, bool stayOnSamePage, Func<ContentItem, Task> conditionallyPublish)
        {
            var contentItem = await _contentManager.GetAsync(contentItemId, VersionOptions.DraftRequired);

            if (contentItem == null)
            {
                return NotFound();
            }

            var model = await _contentItemDisplayManager.UpdateEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, false);
            if (!ModelState.IsValid)
            {
                _session.CancelAsync();
                return View("Edit", model);
            }

            // The content item needs to be marked as saved in case the drivers or the handlers have
            // executed some query which would flush the saved entities inside the above UpdateEditorAsync.
            _session.Save(contentItem);

            await conditionallyPublish(contentItem);

            //if (returnUrl == null)
            //{
            //    return RedirectToAction("Edit", new RouteValueDictionary { { "ContentItemId", contentItem.ContentItemId } });
            //}
            //else if (stayOnSamePage)
            //{
            //    return RedirectToAction("Edit", new RouteValueDictionary { { "ContentItemId", contentItem.ContentItemId }, { "returnUrl", returnUrl } });
            //}
            //else
            //{
            //    return LocalRedirect(returnUrl);
            //}

            return RedirectToRoute(returnUrl);
        }

    }
}
