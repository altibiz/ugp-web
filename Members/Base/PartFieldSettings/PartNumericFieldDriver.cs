using Members.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Admin;
using OrchardCore.ContentFields.Drivers;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentFields.ViewModels;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Members.PartFieldSettings
{
    public class PartNumericFieldDriver : NumericFieldDisplayDriver
    {
        private IHttpContextAccessor _httpCA;

        public PartNumericFieldDriver(IStringLocalizer<NumericFieldDisplayDriver> localizer,IHttpContextAccessor httpContextAccessor) : base(localizer) {
            _httpCA = httpContextAccessor;
        }

        public override IDisplayResult Edit(NumericField field, BuildFieldEditorContext context)
        {
            var fieldDef = DriverService.GetFieldDef(context, AdminAttribute.IsApplied(_httpCA.HttpContext));
            if (fieldDef==null) return null;
            return Initialize<EditNumericFieldViewModel>(GetEditorShapeType(fieldDef), model =>
            {
                var settings = context.PartFieldDefinition.GetSettings<NumericFieldSettings>();
                model.Value = context.IsNew ? settings.DefaultValue : Convert.ToString(field.Value, CultureInfo.CurrentUICulture);

                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = fieldDef;
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(NumericField field, IUpdateModel updater, UpdateFieldEditorContext context)
        {
            var fieldDef = DriverService.GetFieldDef(context, AdminAttribute.IsApplied(_httpCA.HttpContext));
            if (fieldDef == null) return null;
            if (fieldDef.Editor() == "Disabled") return Edit(field, context);
            return await base.UpdateAsync(field, updater, context);
        }
    }
}
