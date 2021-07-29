using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class CreateMemberModel : PageModel
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

        public CreateMemberModel(IContentManager contentManager, IUserService userService, IContentDefinitionManager contentDefinitionManager, IContentItemDisplayManager contentItemDisplayManager, IHtmlLocalizer<CreateMemberModel> htmlLocalizer, INotifier notifier, ISession session, IUpdateModelAccessor updateModelAccessor)
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
        
        public async Task OnGetAsync(string id = contentType)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                NotFound();
            }

            var contentItem = await _contentManager.NewAsync(id);

            var model = await _contentItemDisplayManager.BuildEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);
            ContentItem = model;
        }


        public async Task<IActionResult> OnPostCreateMemberAsync([Bind(Prefix = "submit.CreateMember")] string submitCreateMember, string returnUrl, string id = contentType)
        {
            var user = await _userService.GetAuthenticatedUserAsync(User) as OrchardCore.Users.Models.User;
            returnUrl = "/Members/portal";

            ViewData["ReturnUrl"] = returnUrl;

            var stayOnSamePage = submitCreateMember == "submit.CreateMemberAndContinue";
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

        [HttpPost, ActionName("CreateMember")]
        [FormValueRequired("submit.CreateMemberToCompany")]
        public async Task<IActionResult> OnPostCreateMemberToCompanyAsync([Bind(Prefix = "submit.CreateMemberToCompany")] string submitCreate, string returnUrl, string id = contentType)
        {
            var stayOnSamePage = submitCreate == "submit.CreateMemberAndContinue";

            returnUrl = "/Members/CreateCompany";
            // pass a dummy content to the authorization check to check for "own" variations
            var dummyContent = await _contentManager.NewAsync(id);
            ViewData["ReturnUrl"] = returnUrl;

            return await CreatePOST(id, returnUrl, stayOnSamePage, async contentItem =>
            {

                await _contentManager.PublishAsync(contentItem);

                var typeDefinition = _contentDefinitionManager.GetTypeDefinition(contentItem.ContentType);

                _notifier.Success(string.IsNullOrWhiteSpace(typeDefinition.DisplayName)
                    ? H["Tvoj sadržaj je objavljen."]
                    : H["Tvoj {0} sadržaj je objavljen.", typeDefinition.DisplayName]);
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

                ContentItem = model;
                return Page();
            }

            await _contentManager.CreateAsync(contentItem, VersionOptions.Draft);

            await conditionallyPublish(contentItem);

            if ((!string.IsNullOrEmpty(returnUrl)) && (!stayOnSamePage))
            {
                return RedirectToPage("CreateCompany", "CreateCompanyNewMember");
               // return LocalRedirect(returnUrl);
            }

            return RedirectToRoute(returnUrl);
        }
        
        [HttpGet]
        public async Task Edit(string contentItemId)
        {
            var contentItem = await _contentManager.GetAsync(contentItemId, VersionOptions.Latest);

            if (contentItem == null)
                NotFound();

            var model = await _contentItemDisplayManager.BuildEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, false);

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
                    ? H["Tvoj sadržaj je objavljen."]
                    : H["Tvoj {0} sadržaj je objavljen.", typeDefinition.DisplayName]);
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

                ContentItem = model;
                return Page();
            }

            // The content item needs to be marked as saved in case the drivers or the handlers have
            // executed some query which would flush the saved entities inside the above UpdateEditorAsync.
            _session.Save(contentItem);

            await conditionallyPublish(contentItem);
            return RedirectToRoute(returnUrl);
        }
    }
}
