using Members.PartFieldSettings;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.Taxonomies.Fields;
using System;

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

        public TaxonomyField ContribType { get; set; }

        public TextField Skills { get; set; }

        public string LegalName
        {
            get => Name?.Text + (string.IsNullOrEmpty(Surname?.Text) ? "" : " " + Surname?.Text);
        }

        public string OldSalt { get; set; }
        public string OldHash { get; set; }

        public DateField MembershipExpiry { get; set; }

        public bool IsMember
        {
            get
            {
                if (MembershipExpiry == null || !MembershipExpiry.Value.HasValue) return false;
                return MembershipExpiry.Value.GetValueOrDefault() > DateTime.Today;
            }
        }
    }

    public class PersonPartSettings : IFieldEditorSettings
    {
        public PersonType? Type { get; set; }

        public FieldSettingsExt GetFieldSettings(string propertyName, string label, bool isNew, bool isAdminTheme)
        {
            if (isAdminTheme) return default;
            return new(false,
                propertyName == nameof(PersonPart.Surname) && Type == PersonType.Legal
                || propertyName == nameof(PersonPart.MembershipExpiry),
                propertyName switch
                {
                    nameof(PersonPart.Name) => Type == PersonType.Legal ? "Naziv" : label,
                    _ => label
                });
        }
    }
}
