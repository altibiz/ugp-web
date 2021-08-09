using Members.Persons;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Lists.Models;
using OrchardCore.Taxonomies.Settings;
using OrchardCore.Title.Models;

namespace Members.Core
{
    public static class MemberExtensions
    {
        public static void ExecuteMemberMigrations(this IContentDefinitionManager _contentDefinitionManager)
        {
            #region MemberType
            _contentDefinitionManager.AlterTypeDefinition("Member", type => type
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
            ); ; ;

            #endregion

            #region MemberPart
            _contentDefinitionManager.AlterPartDefinition("Member", part => part

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
                .WithField("Activity", field => field
                    .OfType("TaxonomyField")
                    .WithDisplayName("Želite li aktivno doprinijeti radu udruge?")
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
                .WithField("Skills", field => field
                    .OfType("TextField")
                    .WithDisplayName("Opišite vaše vještine/znanja")
                    .WithEditor("TextArea")
                    .WithPosition("9")
                    ));
            #endregion

            #region CompanyType
            _contentDefinitionManager.AlterTypeDefinition("Company", type => type
                .DisplayedAs("Tvrtka")
                .Creatable()
                .Listable()
                .Securable()
                    .WithPart("PersonPart", part => part
                    .WithPosition("0")
                    .WithSettings(new PersonPartSettings
                    {
                        Type = PersonType.Legal
                    })
                )
                .WithPart("Company", part => part
                    .WithPosition("1")
                )
                .WithPart("AliasPart", part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedDisabled,
                        Pattern = "{{ ContentItem.Content.PersonPart.Name.Text }}",
                    })
                )
            ); ;
            #endregion

            #region CompanyPart
            _contentDefinitionManager.AlterPartDefinition("Company", part => part
                .WithField("AuthorizedRepresentative", field => field
                    .OfType("TextField")
                    .WithDisplayName("Ovlaštena osoba za zastupanje")
                    .WithPosition("4")
                    .WithSettings(new TextFieldSettings
                    {
                        Required = true,
                    })
                )
                .WithField("TurnoverIn2019", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Promet u 2019")
                    .WithPosition("5")
                )
                .WithField("EmployeeNumber", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Broj zaposlenih")
                    .WithPosition("6")
                )
                .WithField("OrganisationType", field => field
                    .OfType("TaxonomyField")
                    .WithDisplayName("Vrsta organizacije/Pravni oblik")
                    .WithEditor("Tags")
                    .WithDisplayMode("Tags")
                    .WithPosition("7")
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
                .WithField("Function", field => field
                    .OfType("TaxonomyField")
                    .WithDisplayName("Funkcija")
                    .WithEditor("Tags")
                    .WithDisplayMode("Tags")
                    .WithPosition("8")
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
                    .WithPosition("9")
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
                    .WithPosition("10")
                )
            );

            #endregion
        }
    }
}
