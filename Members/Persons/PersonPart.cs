using Members.PartFieldSettings;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.Taxonomies.Fields;

namespace Members.Persons
{

    public enum PersonType
    {
        Natural,
        Legal
    }

    public class PersonPart : ContentPart
    {
        public TextField Oib { get; set; }

        public TextField Name { get; set; }

        public TextField Surname { get; set; }

        public TextField Address { get; set; }

        public TextField City { get; set; }

        public TaxonomyField County { get; set; }

        public TextField Phone { get; set; }

        public TextField Email { get; set; }

        public string LegalName
        {
            get => Name?.Text + (string.IsNullOrEmpty(Surname?.Text) ? "" : " " + Surname?.Text);
        }
    }

    public class PersonPartSettings : IFieldEditorSettings
    {
        public PersonType? Type { get; set; }

        public DisplayModeResult GetFieldDisplayMode(string propertyName, string displayMode, BuildFieldEditorContext context, bool isAdminTheme)
        {
            if (isAdminTheme) return displayMode;
            if (propertyName == nameof(PersonPart.Surname) && Type == PersonType.Legal) return false;

            return context.IsNew ? displayMode : "Disabled";
        }

        public string GetFieldLabel(string propertyName, string displayName, bool isAdminTheme)
        {
            return propertyName switch
            {
                nameof(PersonPart.Name) => Type == PersonType.Legal ? "Naziv" : displayName,
                _ => displayName
            };
        }
    }
}
