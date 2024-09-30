using Etch.OrchardCore.Fields.Colour.Settings;
using Etch.OrchardCore.Fields.Colour.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Settings;
using System.Text.Json;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Fields.Colour.Drivers
{
    public class ColourSettingsDisplayDriver : SectionDisplayDriver<ISite, ColourSettings>
    {
        #region Dependencies

        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructor

        public ColourSettingsDisplayDriver(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor)
        {
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Overrides

        public override async Task<IDisplayResult> EditAsync(ISite model, ColourSettings section, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (!await _authorizationService.AuthorizeAsync(user, Permissions.ManageColourSettings))
            {
                return null;
            }

            return Initialize<ColourSettingsViewModel>("ColourSettings_Edit", model =>
            {
                model.Colours = JsonSerializer.Serialize(section.Colours);
            }).Location("Content:3").OnGroup(Constants.GroupId);
        }


        public async override Task<IDisplayResult> UpdateAsync(ISite model, ColourSettings section, UpdateEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (!await _authorizationService.AuthorizeAsync(user, Permissions.ManageColourSettings))
            {
                return null;
            }

            if (context.GroupId == Constants.GroupId)
            {
                var colors = new ColourSettingsViewModel();

                if (await context.Updater.TryUpdateModelAsync(model, Prefix))
                {
                    section.Colours = JsonSerializer.Deserialize<ColourItem[]>(colors.Colours);
                }
            }

            return await EditAsync(model, section, context);
        }

        #endregion
    }
}