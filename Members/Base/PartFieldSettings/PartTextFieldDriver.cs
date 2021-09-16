using Members.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Admin;
using OrchardCore.ContentFields.Drivers;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.ViewModels;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace Members.PartFieldSettings
{
    public class PartTextFieldDriver : TextFieldDisplayDriver
    {
        private IHttpContextAccessor _httpCA;

        public PartTextFieldDriver(IStringLocalizer<TextFieldDisplayDriver> localizer,IHttpContextAccessor httpContextAccessor) : base(localizer) {
            _httpCA = httpContextAccessor;
        }

        public override IDisplayResult Edit(TextField field, BuildFieldEditorContext context)
        {
            if (!DriverService.CheckSettings(context, AdminAttribute.IsApplied(_httpCA.HttpContext))) return null;
            var shapeType = GetEditorShapeType(context);
            return Initialize<EditTextFieldViewModel>(shapeType, model =>
            {
                model.Text = field.Text;
                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = context.PartFieldDefinition;
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(TextField field, IUpdateModel updater, UpdateFieldEditorContext context)
        {
            if (!DriverService.CheckSettings(context, AdminAttribute.IsApplied(_httpCA.HttpContext))) return null;
            if (context.PartFieldDefinition.Editor() == "Disabled") return Edit(field, context);
            return await base.UpdateAsync(field, updater, context);
        }
    }
}
