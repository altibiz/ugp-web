using Microsoft.Extensions.Localization;
using Etch.OrchardCore.Fields.ResponsiveMedia.Fields;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;
using Etch.OrchardCore.Fields.ResponsiveMedia.Utils;
using OrchardCore.Media;
using System.Text.Json;
using OrchardCore.DisplayManagement.Handlers;

namespace Etch.OrchardCore.Fields.ResponsiveMedia.Settings
{
    public class ResponsiveMediaFieldSettingsDriver : ContentPartFieldDefinitionDisplayDriver<ResponsiveMediaField>
    {
        #region Dependencies

        private readonly IMediaFileStore _mediaFileStore;
        private readonly IStringLocalizer<ResponsiveMediaFieldSettingsDriver> T;

        #endregion

        #region Constructor

        public ResponsiveMediaFieldSettingsDriver(IStringLocalizer<ResponsiveMediaFieldSettingsDriver> localizer, IMediaFileStore mediaFileStore)
        {
            T = localizer;
            _mediaFileStore = mediaFileStore;
        }

        #endregion

        public override IDisplayResult Edit(ContentPartFieldDefinition model,BuildEditorContext context)
        {
            return Initialize<ResponsiveMediaFieldSettings>("ResponsiveMediaFieldSettings_Edit", viewModel => {
                var settings = model.GetSettings<ResponsiveMediaFieldSettings>();
                viewModel.AllowMediaText = settings.AllowMediaText;
                viewModel.Multiple = settings.Multiple;
                viewModel.Required = settings.Required;
                viewModel.Hint = settings.Hint;
                viewModel.FallbackData = settings.FallbackData;
                viewModel.Breakpoints = settings.Breakpoints;
            })
                .Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentPartFieldDefinition model, UpdatePartFieldEditorContext context)
        {
            var viewModel = new UpdateResponsiveMediaFieldSettingsViewModel();

            await context.Updater.TryUpdateModelAsync(viewModel, Prefix);

            var settings = new ResponsiveMediaFieldSettings
            {
                AllowMediaText = viewModel.AllowMediaText,
                Hint = viewModel.Hint,
                Multiple = viewModel.Multiple,
                Required = viewModel.Required,
                FallbackData = JsonSerializer.Serialize(ResponsiveMediaUtils.ParseMedia(_mediaFileStore, viewModel.FallbackData))
            };

            try
            {
                settings.Breakpoints = viewModel.Breakpoints;
                settings.GetBreakpoints();
            } catch
            {
                context.Updater.ModelState.AddModelError(Prefix, T["Failed to parse breakpoints, make sure it only contains numeric values."]);
            }

            context.Builder.WithSettings(settings);

            return Edit(model, context);
        }
    }
}
