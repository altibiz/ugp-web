using Json.More;
using Members.PartFieldSettings;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentManagement.Metadata.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace Members.Base
{
    public class DriverService
    {
        private static IEnumerable<Type> _implementingTypes;
        private static IEnumerable<Type> implementingTypes = _implementingTypes ??= AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.FullName.Contains("Microsoft"))
                    .SelectMany(x => x.GetTypes())
                    .Where(t => typeof(IFieldEditorSettings).IsAssignableFrom(t) && t.IsClass).ToList();

        public static IEnumerable<Type> ImplementingTypes { get => implementingTypes; set => implementingTypes = value; }

        public static ContentPartFieldDefinition GetFieldDef(BuildFieldEditorContext context, bool isAdminTheme)
        {
            IFieldEditorSettings partSettings = null;
            var oldDef = context.PartFieldDefinition;
            foreach (var typ in ImplementingTypes)
            {
                context.TypePartDefinition.Settings.TryGetPropertyValue(typ.Name, out JsonNode val);
                if (val == null) continue;
                partSettings = val.ToObject(typ) as IFieldEditorSettings;
            }
            if (partSettings != null)
            {
                var textset = oldDef.GetSettings<ContentPartFieldSettings>();
                var newEditor = partSettings.GetFieldDisplayMode(context.PartFieldDefinition.Name, textset.Editor, context, isAdminTheme);
                if (!newEditor.IsVisible)
                    return null;
                var newDispName = partSettings.GetFieldLabel(context.PartFieldDefinition.Name, textset.DisplayName, isAdminTheme);
                if (textset.Editor != newEditor || textset.DisplayName != newDispName)
                {
                    var newDef = new ContentPartFieldDefinition(oldDef.FieldDefinition, oldDef.Name, oldDef.Settings)
                    {
                        PartDefinition = oldDef.PartDefinition
                    };
                    var newSett = newDef.GetSettings<ContentPartFieldSettings>();
                    newSett.Editor = newEditor;
                    newSett.DisplayName = newDispName;
                    return newDef;
                }

            }
            return oldDef;
        }

    }
}