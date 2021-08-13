using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentFields.Drivers;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.ViewModels;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Taxonomies.Drivers;
using OrchardCore.Taxonomies.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Members.PartFieldSettings
{
    public class PartTaxonomyFieldDriver : TaxonomyFieldDisplayDriver
    {
        private static IEnumerable<Type> _implementingTypes;
        public static IEnumerable<Type> ImplementingTypes = _implementingTypes ??= AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(t => typeof(IFieldEditorSettings).IsAssignableFrom(t) && t.IsClass).ToList();

        public PartTaxonomyFieldDriver(IContentManager cm,IStringLocalizer<TaxonomyFieldDisplayDriver> localizer) : base(cm,localizer) { }

        private bool CheckSettings(BuildFieldEditorContext context)
        {
            IFieldEditorSettings partSettings = null;
            foreach (var typ in ImplementingTypes)
            {
                context.TypePartDefinition.Settings.TryGetValue(typ.Name, out JToken val);
                if (val == null) continue;
                partSettings = val.ToObject(typ) as IFieldEditorSettings;
            }
            if (partSettings != null)
            {
                if (partSettings.IsFieldHidden(context.PartFieldDefinition.Name, context))
                    return false;
                var textset = context.PartFieldDefinition.GetSettings<ContentPartFieldSettings>();
                textset.DisplayName = partSettings.GetFieldLabel(context.PartFieldDefinition.Name, textset.DisplayName);
                textset.Editor = partSettings.GetFieldEditor(context.PartFieldDefinition.Name, textset.DisplayMode, context);
            }
            return true;
        }

        public override IDisplayResult Edit(TaxonomyField field, BuildFieldEditorContext context)
        {
            if (!CheckSettings(context)) return null;
            return base.Edit(field, context);
        }

        public override async Task<IDisplayResult> UpdateAsync(TaxonomyField field, IUpdateModel updater, UpdateFieldEditorContext context)
        {
            if (!CheckSettings(context)) return null;
            if (context.PartFieldDefinition.Editor() == "Disabled") return Edit(field, context);
            return await base.UpdateAsync(field, updater, context);
        }
    }
}
