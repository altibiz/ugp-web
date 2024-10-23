using OrchardCore.ContentManagement.Display.Models;

namespace Members.PartFieldSettings
{
    public struct FieldSettingsExt
    {
        public bool Disabled { get; set; }
        public bool Hidden { get; set; }
        public string Label { get; set; }

        public FieldSettingsExt(bool disabled, bool hidden, string name)
        {
            Disabled = disabled;
            Hidden = hidden;
            Label = name;
        }

    }

    public interface IFieldEditorSettings
    {

        /// <summary>
        /// Get how field is displayed
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="displayMode"></param>
        /// <param name="context"></param>
        /// <returns>default, Disabled for disabled or null for hidden</returns>
        FieldSettingsExt GetFieldSettings(string propertyName, string label, bool isNew, bool isAdminTheme);
    }
}