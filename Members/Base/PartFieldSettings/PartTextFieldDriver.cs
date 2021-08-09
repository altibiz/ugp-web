using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentFields.Drivers;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentFields.ViewModels;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Members.PartFieldSettings
{
    public class PartTextFieldDriver : TextFieldDisplayDriver
    {
        private static IEnumerable<Type> _implementingTypes;
        public static IEnumerable<Type> ImplementingTypes = _implementingTypes ??= AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(t => typeof(IFieldSettings).IsAssignableFrom(t) && t.IsClass).ToList();

        public PartTextFieldDriver(IStringLocalizer<TextFieldDisplayDriver> localizer) : base(localizer) { }

        private bool CheckSettings(BuildFieldEditorContext context)
        {
            IFieldSettings partSettings = null;
            foreach (var typ in ImplementingTypes)
            {
                context.TypePartDefinition.Settings.TryGetValue(typ.Name, out JToken val);
                if (val == null) continue;
                partSettings = val.ToObject(typ) as IFieldSettings;
            }
            if (partSettings != null)
            {
                if (partSettings.IsFieldHidden(context.PartFieldDefinition.Name, context.ContentPart))
                    return false;
                var textset = context.PartFieldDefinition.GetSettings<ContentPartFieldSettings>();
                textset.DisplayName = partSettings.GetFieldLabel(context.PartFieldDefinition.Name, textset.DisplayName);
            }
            return true;
        }

        public override IDisplayResult Edit(TextField field, BuildFieldEditorContext context)
        {
            if (!CheckSettings(context)) return null;
            return Initialize<EditTextFieldViewModel>(GetEditorShapeType(context), model =>
            {
                model.Text = field.Text;
                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = context.PartFieldDefinition;
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(TextField field, IUpdateModel updater, UpdateFieldEditorContext context)
        {
            if (!CheckSettings(context)) return null;
            return await base.UpdateAsync(field, updater, context);
        }
    }
}
