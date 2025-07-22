using Members.PartFieldSettings;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Taxonomies.Fields;
using OrchardCore.Taxonomies.Settings;
using OrchardCore.Title.Models;
using System.Threading.Tasks;

namespace Members.Payments
{
    public class Pledge : ContentPart
    {
        public TextField PayerName { get; set; }
        public TextField Oib { get; set; }
        public TaxonomyField Variant { get; set; }
        public NumericField Amount { get; set; }
        public TextField ReferenceNr { get; set; }
        public ContentPickerField Person { get; set; }

        public TextField Email { get; set; }

        public TextField Note { get; set; }
    }

    public class PledgeForm : ContentPart
    {
        public ContentPickerField PledgeVariant { get; set; }
    }

    public class PledgeSettings : IFieldEditorSettings
    {
        public FieldSettingsExt GetFieldSettings(string propertyName, string label, bool isNew, bool isAdminTheme)
        {
            if (isAdminTheme) return default;
            return new(false,
                propertyName == nameof(Pledge.Person) || propertyName == nameof(Pledge.ReferenceNr)
                || propertyName == nameof(Pledge.Amount) || propertyName == nameof(Pledge.Note),
                label);
        }
    }

    public class PledgeVariant : ContentPart
    {
        public NumericField Price { get; set; }

        public TextField ReferenceNrPrefix { get; set; }
    }

    public static class PledgeMigrations
    {
        public static async Task CreatePledge(this IContentDefinitionManager _contentDefinitionManager)
        {
            await _contentDefinitionManager.AlterTypeDefinitionAsync("Pledge", type => type
                .DisplayedAs("Uplatnica")
                .Listable()
                .Securable()
                .Creatable()
                .WithPart(nameof(Pledge), part => part
                    .WithPosition("0")
                    .WithSettings(new PledgeSettings())
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("1")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedDisabled,
                        Pattern = "{{ ContentItem.Content.Pledge.PayerName.Text }} - {{ ContentItem.Content.Pledge.Amount.Value | format_number: \"C\"  }}",
                    })
                )
            );
            await _contentDefinitionManager.AlterPartDefinitionAsync(nameof(Pledge), part => part
                .WithField("Amount", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Iznos")
                    .WithPosition("0")
                    .WithSettings(new NumericFieldSettings
                    {
                        Scale = 2
                    })
                )
                .WithField("PayerName", field => field
                    .OfType("TextField")
                    .WithDisplayName("Platitelj")
                    .WithPosition("1")
                    .WithSettings(new TextFieldSettings
                    {
                        Required = true,
                    })
                )
                .WithField("Note", field => field
                    .OfType("TextField")
                    .WithDisplayName("Opis plaćanja")
                    .WithPosition("9")
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
                .WithField("Oib", field => field
                    .OfType("TextField")
                    .WithDisplayName("OIB")
                    .WithPosition("1")
                    .WithSettings(new TextFieldSettings
                    {
                        Required = true,
                    })
                )
                .WithField("ReferenceNr", field => field
                    .OfType("TextField")
                    .WithDisplayName("Poziv na broj")
                    .WithPosition("3")
                )
                .WithField("Variant", field => field
                    .OfType("TaxonomyField")
                    .WithEditor("Tags")
                    .WithDisplayMode("Tags")
                    .WithDisplayName("Količina")
                    .WithPosition("6")
                    .WithSettings(new TaxonomyFieldSettings
                    {
                        TaxonomyContentItemId = "5599209fa3d04b0da7482e655",
                        Unique = true,
                        Required = true
                    })
                    .WithSettings(new TaxonomyFieldTagsEditorSettings
                    {
                        Open = false,
                    })
                )
                .WithField("Person", field => field
                    .OfType("ContentPickerField")
                    .WithDisplayName("Član")
                    .WithSettings(new ContentPickerFieldSettings
                    {
                        DisplayedContentTypes = ["Member", "Company"],
                    })
                )
           );

            await _contentDefinitionManager.AlterTypeDefinitionAsync("PledgeVariant", type => type
                 .DisplayedAs("Vrijednost uplate")
                 .Creatable()
                 .WithPart(nameof(PledgeVariant), part => part
                     .WithPosition("0")
                 )
                 .WithPart("TitlePart", part => part
                 .WithPosition("1")
                     .WithSettings(new TitlePartSettings
                     {
                         Options = TitlePartOptions.EditableRequired,
                     })
                  )
             );

            await _contentDefinitionManager.UpdatePledgeVariant();

            await _contentDefinitionManager.AlterTypeDefinitionAsync("PledgeForm", type => type.Stereotype("Widget"));

        }

        public async static Task UpdatePledgeVariant(this IContentDefinitionManager cdm)
        {
            await cdm.AlterPartDefinitionAsync(nameof(PledgeVariant), part => part
            .WithField("Price", field => field
                .OfType("NumericField")
                .WithDisplayName("Cijena")
                .WithPosition("0")
                .WithSettings(new NumericFieldSettings
                {
                    Required = true,
                    Scale = 2
                })
            )
            .WithField("ReferenceNrPrefix", field => field
                    .OfType("TextField")
                    .WithDisplayName("Prefix poziv na broja")
                    .WithPosition("2")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Prefix koji će se koristiti za poziv na broj uplatnice. Za obicne clanove 12-, za punopravne clanove 13-",
                    })
                )
            );
        }

        public async static Task UpdatePledgeForm(this IContentDefinitionManager _contentDefinitionManager)
        {
            await _contentDefinitionManager.AlterPartDefinitionAsync("PledgeForm", part => part
            .WithField("PledgeVariant", field => field
                .OfType("ContentPickerField")
                .WithDisplayName("Vrsta uplata")
                .WithSettings(new ContentPickerFieldSettings
                {
                    Required = false,
                    Multiple = false,
                    DisplayAllContentTypes = false,
                    DisplayedContentTypes =
                    [
                        "Taxonomy",
                    ],
                    TitlePattern = "{ { Model.ContentItem | display_text } }",
                })));
        }


    }
}
