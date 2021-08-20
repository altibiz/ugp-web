using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Taxonomies.Fields;
using OrchardCore.Taxonomies.Settings;

namespace Members.Core
{
    public class Payment : ContentPart
    {
        public NumericField Amount { get; set; }
        public TextField PayerName { get; set; }
        public TextField Address { get; set; }
        public TextField ReferenceNr { get; set; }
        public TextField PaymentRef { get; set; }
        public TaxonomyField ProcessState { get; set; }
        public DateField PaymentDate { get; set; }
        public ContentPickerField Member { get; set; }
    }


    public static class PaymentMigration
    {
        public static void MigratePayment(this IContentDefinitionManager _contentDefinitionManager)
        {
            _contentDefinitionManager.AlterTypeDefinition("Payment", type => type
                .DisplayedAs("Donacija")
                .Creatable()
                .Listable()
                .Securable()
                .WithPart("Payment", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("Payment", part => part
                .WithField("Amount", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Iznos")
                    .WithPosition("0")
                    .WithSettings(new NumericFieldSettings
                    {
                        Required = true,
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
                .WithField("Address", field => field
                    .OfType("TextField")
                    .WithDisplayName("Mjesto")
                    .WithPosition("2")
                )
                .WithField("ReferenceNr", field => field
                    .OfType("TextField")
                    .WithDisplayName("Poziv na broj")
                    .WithPosition("3")
                )
                .WithField("PaymentRef", field => field
                    .OfType("TextField")
                    .WithDisplayName("Referenca plaćanja")
                    .WithPosition("4")
                )
                .WithField("ProcessState", field => field
                    .OfType("TaxonomyField")
                    .WithDisplayName("Stanje obrade")
                    .WithPosition("5")
                    .WithSettings(new TaxonomyFieldSettings
                    {
                        TaxonomyContentItemId = "4qpszaf4724qh6kxf7ek4rbcmv",
                        Unique = true,
                    })
                )
                .WithField("PaymentDate", field => field
                    .OfType("DateField")
                    .WithDisplayName("Datum plaćanja")
                )
                .WithField("Member", field => field
                    .OfType("ContentPickerField")
                    .WithDisplayName("Član")
                    .WithSettings(new ContentPickerFieldSettings
                    {
                        DisplayedContentTypes = new[] { "Member", "Company" },
                    })
                )
            );
        }
    }
}
