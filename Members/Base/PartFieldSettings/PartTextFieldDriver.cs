using Members.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Admin;
using OrchardCore.ContentFields.Drivers;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace Members.PartFieldSettings
{
    public class PartTextFieldDriver : ContentFieldDisplayDriver<TextField>
    {
        private IHttpContextAccessor _httpCA;
        private TextFieldDisplayDriver _driver;

        public PartTextFieldDriver(IStringLocalizer<TextFieldDisplayDriver> localizer, IHttpContextAccessor httpContextAccessor)
        {
            _httpCA = httpContextAccessor;
            _driver = new TextFieldDisplayDriver(localizer);
        }

        public override IDisplayResult Display(TextField field, BuildFieldDisplayContext context)
        {
            return _driver.Display(field, context);
        }

        public override IDisplayResult Edit(TextField field, BuildFieldEditorContext context)
        {
            var fieldDef = DriverService.GetFieldDef(context, AdminAttribute.IsApplied(_httpCA.HttpContext));
            if (fieldDef == null) return null;
            return Initialize<EditTextFieldViewModel>(GetEditorShapeType(fieldDef), model =>
            {
                model.Text = field.Text;
                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = fieldDef;
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(TextField field, UpdateFieldEditorContext context)
        {
            var fieldDef = DriverService.GetFieldDef(context, AdminAttribute.IsApplied(_httpCA.HttpContext));
            if (fieldDef == null) return null;
            if (fieldDef.Editor() == "Disabled") return Edit(field, context);
            return await _driver.UpdateAsync(field, context);
        }
    }
}
