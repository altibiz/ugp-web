using Members.PartFieldSettings;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.Taxonomies.Fields;

namespace Members.Core
{
    public class Member : ContentPart
    {
        public UserPickerField User { get; set; }

        public DateField DateOfBirth { get; set; }

        public TaxonomyField Sex { get; set; }

        public TextField AdminNotes { get; set; }
    }

    public class MemberSettings : IFieldEditorSettings
    {
        public FieldSettingsExt GetFieldSettings(string propertyName, string labelName, bool isNew, bool isAdminTheme)
        {
            return new(!isNew, !isAdminTheme && propertyName == nameof(Member.AdminNotes), labelName);
        }
    }


}
