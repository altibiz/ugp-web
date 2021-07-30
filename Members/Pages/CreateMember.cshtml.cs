using System.Threading.Tasks;
using Members.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Users.Services;
using YesSql;

namespace Members.Pages
{
    [Authorize]
    public class CreateMemberModel : PageModel
    {
        private const string contentType = "Member";

        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IHtmlLocalizer H;
        private readonly dynamic New;
        private readonly INotifier _notifier;
        private readonly ISession _session;
        private readonly IUpdateModelAccessor _updateModelAccessor;
        private readonly IUserService _userService;

        public IShape Shape { get; set; }

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

        public async Task OnGetAsync()
        {

            var contentItem = await _contentManager.NewAsync(contentType);
            var model = await _contentItemDisplayManager.BuildEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);
            Shape = model;
        }


        public async Task<IActionResult> OnPostCreateMemberAsync()
        {
            return await CreatePOST("Portal");
        }
        public async Task<IActionResult> OnPostCreateMemberToCompanyAsync()
        {
            return await CreatePOST("CreateCompany");
        }

        private async Task<IActionResult> CreatePOST(string nextPage)
        {
            var contentItem = await _contentManager.NewAsync(contentType);
            var model = await _contentItemDisplayManager.UpdateEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);

            if (!ModelState.IsValid)
            {
                await _session.CancelAsync();

                Shape = model;
                return Page();
            }

            var user = await _userService.GetAuthenticatedUserAsync(User) as OrchardCore.Users.Models.User;

            // Set the current user as the owner to check for ownership permissions on creation
            contentItem.Owner = User.Identity.Name;

            await _contentManager.CreateAsync(contentItem, VersionOptions.Draft);

            var member = contentItem.As<Member>();

            member.User.UserIds=new[] { user.UserId };

            await _contentManager.PublishAsync(contentItem);

            _notifier.Success(H["Member registration successful"]);
                return RedirectToPage(nextPage);
        }
    }
}
