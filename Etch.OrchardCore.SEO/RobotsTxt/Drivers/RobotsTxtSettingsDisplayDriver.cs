using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Etch.OrchardCore.SEO.RobotsTxt.Models;
using Etch.OrchardCore.SEO.RobotsTxt.ViewModels;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Settings;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.RobotsTxt.Drivers
{
    public class RobotsTxtSettingsDisplayDriver : SectionDisplayDriver<ISite, RobotsTxtSettings>
    {
        #region Constants

        public const string GroupId = "robotstxt";

        #endregion

        #region Dependencies

        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructor

        public RobotsTxtSettingsDisplayDriver(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor)
        {
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Overrides

        public override async Task<IDisplayResult> EditAsync(ISite model, RobotsTxtSettings section, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (!await _authorizationService.AuthorizeAsync(user, Permissions.ManageRobotsTxt))
            {
                return null;
            }

            return Initialize<RobosTxtSettingsViewModel>("RobotsTxtSettings_Edit", model =>
            {
                model.CustomContent = section.CustomContent;
                model.Mode = section.Mode;
                model.NoIndex = section.NoIndex;
            }).Location("Content:3").OnGroup(GroupId);
        }

        public override async Task<IDisplayResult> UpdateAsync(ISite model, RobotsTxtSettings section, UpdateEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (!await _authorizationService.AuthorizeAsync(user, Permissions.ManageRobotsTxt))
            {
                return null;
            }

            if (context.GroupId == GroupId)
            {
                var smodel = new RobosTxtSettingsViewModel();

                await context.Updater.TryUpdateModelAsync(smodel, Prefix);

                section.CustomContent = smodel.CustomContent;
                section.Mode = smodel.Mode;
                section.NoIndex = smodel.NoIndex;
            }

            return await EditAsync(model, section, context);
        }

        #endregion
    }
}
