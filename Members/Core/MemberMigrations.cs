﻿using Members.Persons;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Lists.Models;
using OrchardCore.Media.Settings;
using OrchardCore.Taxonomies.Settings;
using OrchardCore.Title.Models;
using System.Threading.Tasks;

namespace Members.Core
{
    public static class MemberMigrations
    {
        public static async Task ExecuteMemberMigrations(this IContentDefinitionManager _contentDefinitionManager)
        {
            #region Member Content Type
            await _contentDefinitionManager.AlterTypeDefinitionAsync("Member", type => type
                .DisplayedAs("Član")
                .Creatable()
                .Listable()
                .Securable()
                .WithPart("PersonPart", part => part.WithPosition("0").
                WithSettings(new PersonPartSettings
                {
                    Type = PersonType.Natural
                }
                ))
                .WithPart("Member", part => part
                    .WithPosition("0")
                    .WithSettings(new MemberSettings())
                )
                .WithPart("AliasPart", part => part
                    .WithPosition("2")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("1")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedDisabled,
                        Pattern = "{{ ContentItem.Content.PersonPart.Name.Text }} {{ ContentItem.Content.PersonPart.Surname.Text }}",
                    })
                )
                .WithPart("ListPart", part => part
                    .WithPosition("3")
                    .WithSettings(new ListPartSettings
                    {
                        PageSize = 10,
                        ContainedContentTypes = new[] { "Company" },
                    })
                )
            );

            #endregion

            #region Member Content Part
            await _contentDefinitionManager.AlterPartDefinitionAsync("Member", part => part

               .WithField("DateOfBirth", field => field
                    .OfType("DateField")
                    .WithDisplayName("Datum rođenja")
                    .WithPosition("3")
                )
                .WithField("User", field => field
                    .OfType("UserPickerField")
                    .WithDisplayName("User")
                    .WithPosition("11")
                    .WithSettings(new UserPickerFieldSettings
                    {
                        DisplayAllUsers = true,
                        DisplayedRoles = new string[] { },
                    })
                )
                .WithField("Sex", field => field
                    .OfType("TaxonomyField")
                    .WithDisplayName("Spol")
                    .WithEditor("Tags")
                    .WithDisplayMode("Tags")
                    .WithPosition("8")
                    .WithSettings(new TaxonomyFieldSettings
                    {
                        TaxonomyContentItemId = "4xgh8bvawx8h2rvyg7vds118w4",
                        Unique = true,
                    })
                )
                .WithField("AdminNotes", field => field
                    .OfType("TextField")
                    .WithDisplayName("Admin bilješke")
                    .WithEditor("TextArea")
                    .WithPosition("12")
                    ));
            #endregion

            #region CompanyType
            await _contentDefinitionManager.AlterTypeDefinitionAsync("Company", type => type
                 .DisplayedAs("Tvrtka")
                 .Creatable()
                 .Listable()
                 .Securable()
                 .WithPart("PersonPart", part => part
                     .WithPosition("0")
                     .WithSettings(new PersonPartSettings
                     {
                         Type = PersonType.Legal,
                     })
                 )
                 .WithPart("Company", part => part
                     .WithPosition("2")
                 )
                 .WithPart("AliasPart", part => part
                     .WithPosition("3")
                 )
                 .WithPart("TitlePart", part => part
                     .WithPosition("1")
                     .WithSettings(new TitlePartSettings
                     {
                         Options = TitlePartOptions.GeneratedDisabled,
                         Pattern = "{{ ContentItem.Content.PersonPart.Name.Text }}",
                     })
                 )
             );
            #endregion

            #region CompanyPart
            await _contentDefinitionManager.AlterPartDefinitionAsync("PersonPart", part => part
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
            );

            await _contentDefinitionManager.AlterPartDefinitionAsync("Company", part => part
                .WithField("AuthorizedRep", field => field
                    .OfType("TextField")
                    .WithDisplayName("Ovlaštena osoba za zastupanje")
                    .WithPosition("1")
                    .WithSettings(new TextFieldSettings
                    {
                        Required = true,
                    })
                )
                .WithField("Revenue2019", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Promet u 2019")
                    .WithPosition("2")
                )
                .WithField("EmployeeNumber", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Broj zaposlenih")
                    .WithPosition("3")
                )
                .WithField("OrganisationType", field => field
                    .OfType("TaxonomyField")
                    .WithDisplayName("Vrsta organizacije/Pravni oblik")
                    .WithEditor("Tags")
                    .WithDisplayMode("Tags")
                    .WithPosition("4")
                    .WithSettings(new TaxonomyFieldSettings
                    {
                        Required = true,
                        TaxonomyContentItemId = "4gy5x0s0wck1p2k2mv19pmwsxw",
                        Unique = true,
                    })
                    .WithSettings(new TaxonomyFieldTagsEditorSettings
                    {
                        Open = false,
                    })
                )
                .WithField("RepRole", field => field
                    .OfType("TaxonomyField")
                    .WithDisplayName("Funkcija")
                    .WithEditor("Tags")
                    .WithDisplayMode("Tags")
                    .WithPosition("5")
                    .WithSettings(new TaxonomyFieldSettings
                    {
                        Required = true,
                        TaxonomyContentItemId = "4cet7kh16f89cxpk2zp9gz4353",
                        Unique = true,
                    })
                    .WithSettings(new TaxonomyFieldTagsEditorSettings
                    {
                        Open = false,
                    })
                )
                .WithField("Activity", field => field
                    .OfType("TaxonomyField")
                    .WithDisplayName("Djelatnost")
                    .WithEditor("Tags")
                    .WithDisplayMode("Tags")
                    .WithPosition("6")
                    .WithSettings(new TaxonomyFieldSettings
                    {
                        Required = true,
                        TaxonomyContentItemId = "4m514eexhnwqnx4asz7xqadfcz",
                    })
                    .WithSettings(new TaxonomyFieldTagsEditorSettings
                    {
                        Open = false,
                    })
                )
                .WithField("PermanentAssociates", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Broj stalnih suradnika")
                    .WithPosition("7")
                )
                .WithField("Logo", field => field
                    .OfType("MediaField")
                    .WithDisplayName("Logo")
                    .WithPosition("0")
                    .WithEditor("Attached")
                    .WithSettings(new MediaFieldSettings
                    {
                        Multiple = false,
                        AllowMediaText = false,
                    })
                )
            );

            #endregion
        }
    }
}
