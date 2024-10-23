using Members.PartFieldSettings;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.DisplayManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace Members.Base
{
    public static class FieldSettingsExtensions
    {
        private static IEnumerable<Type> _implementingTypes;
        private static IEnumerable<Type> implementingTypes = _implementingTypes ??= AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.FullName.Contains("Microsoft"))
                    .SelectMany(x => x.GetTypes())
                    .Where(t => typeof(IFieldEditorSettings).IsAssignableFrom(t) && t.IsClass).ToList();

        public static IEnumerable<Type> ImplementingTypes { get => implementingTypes; set => implementingTypes = value; }

        public static void SetFieldSettingsExt(this IShape shape, bool isNew, bool isAdminTheme)
        {
            var fieldDef = shape.GetType().GetProperty("PartFieldDefinition").GetValue(shape) as ContentPartFieldDefinition;
            var textFieldSettings = fieldDef.GetSettings<ContentPartFieldSettings>();
            IFieldEditorSettings partSettings = null;
            FieldSettingsExt fieldSettingsExt = new FieldSettingsExt { Label = textFieldSettings.DisplayName };
            foreach (var typ in ImplementingTypes)
            {
                fieldDef.ContentTypePartDefinition.Settings.TryGetPropertyValue(typ.Name, out JsonNode val);
                if (val == null) continue;
                partSettings = val.ToObject(typ) as IFieldEditorSettings;
            }
            if (partSettings != null)
            {
                fieldSettingsExt = partSettings.GetFieldSettings(fieldDef.Name, textFieldSettings.DisplayName, isNew, isAdminTheme);
                if (fieldSettingsExt.Hidden)
                {
                    shape.Metadata.Wrappers.Add("HiddenField");
                }
            }
            shape.Properties["FieldSettingsExt"] = fieldSettingsExt;
        }

    }
}