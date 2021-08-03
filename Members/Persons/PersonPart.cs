using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Taxonomies.Fields;

namespace Members.Models
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

    public class PersonPartSettings
    {
        public PersonType? PersonType { get; set; }

        public bool IsFieldHidden(string propertyName)
        {
            return propertyName == nameof(PersonPart.Surname) && this.PersonType == Models.PersonType.Legal;
        }

        public string GetFieldLabel(string propertyName, string displayName)
        {
            return propertyName == nameof(PersonPart.Name) && PersonType == Models.PersonType.Legal ? "Company Name" : displayName;
        }
    }
}
