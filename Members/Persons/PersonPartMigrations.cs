﻿using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Taxonomies.Settings;
using System.Threading.Tasks;

namespace Members.Persons
{
    public static class PersonPartMigrations
    {
        public static async Task AlterPersonPart(this IContentDefinitionManager contentDefinitionManager)
        {
            await contentDefinitionManager.AlterPartDefinitionAsync("PersonPart", part =>
            part
                .WithField("Name", field => field
                    .OfType("TextField")
                    .WithDisplayName("Ime")
                    .WithPosition("1")
                    .WithSettings(new TextFieldSettings
                    {
                        Required = true,
                    })
                )
                .WithField("Surname", field => field
                    .OfType("TextField")
                    .WithDisplayName("Prezime")
                    .WithPosition("1")
                )
                .WithField("Oib", field => field
                    .OfType("TextField")
                    .WithDisplayName("OIB")
                    .WithPosition("0")
                    .WithSettings(new TextFieldSettings
                    {
                        Required = true,
                    })
                )
                .WithField("Address", field => field
                    .OfType("TextField")
                    .WithDisplayName("Adresa")
                    .WithPosition("4")
                )
                .WithField("City", field => field
                    .OfType("TextField")
                    .WithDisplayName("Grad/Općina")
                    .WithPosition("5")
                )
                .WithField("Phone", field => field
                    .OfType("TextField")
                    .WithDisplayName("Telefon")
                    .WithEditor("Tel")
                    .WithPosition("7")
                )
                .WithField("County", field => field
                    .OfType("TaxonomyField")
                    .WithDisplayName("Županija")
                    .WithEditor("Tags")
                    .WithDisplayMode("Tags")
                    .WithPosition("6")
                    .WithSettings(new TaxonomyFieldSettings
                    {
                        TaxonomyContentItemId = "4d0dew9ar7h9nsbpcs7jg2egwe",
                        Unique = true,
                    })
                    .WithSettings(new TaxonomyFieldTagsEditorSettings
                    {
                        Open = false,
                    })
                )
                .WithField("Email", field => field
                    .OfType("TextField")
                    .WithDisplayName("Email")
                    .WithEditor("Email")
                    .WithSettings(new TextFieldSettings
                    {
                        Required = true,
                    })
                )
                .WithField("ContribType", field => field
                    .OfType("TaxonomyField")
                    .WithDisplayName("Aktivan/neaktivan doprinos radu udruge?")
                    .WithEditor("Tags")
                    .WithDisplayMode("Tags")
                    .WithPosition("10")
                    .WithSettings(new TaxonomyFieldSettings
                    {
                        TaxonomyContentItemId = "4k7n3gw5wm7660vqpm0805hedy",
                        Unique = true,
                    })
                    .WithSettings(new TaxonomyFieldTagsEditorSettings
                    {
                        Open = false,
                    })
                )
                .WithField("Skills", field => field
                    .OfType("TextField")
                    .WithDisplayName("Vještine i znanja")
                    .WithEditor("TextArea")
                    .WithPosition("9")
                    )
                .WithField("MembershipExpiry", field => field
                    .OfType("DateField")
                    .WithDisplayName("Istječe članstvo")
                    .WithPosition("10")
                )

            );
        }
    }
}
