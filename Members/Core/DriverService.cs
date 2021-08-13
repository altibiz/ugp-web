using Members.PartFieldSettings;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Members.Core
{
    public class DriverService
    {
        private static IEnumerable<Type> _implementingTypes;
        public static IEnumerable<Type> ImplementingTypes = _implementingTypes ??= AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(t => typeof(IFieldEditorSettings).IsAssignableFrom(t) && t.IsClass).ToList();

        public static bool CheckSettings(BuildFieldEditorContext context)
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

    }
}