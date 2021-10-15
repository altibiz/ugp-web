using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Taxonomies.Fields;
using OrchardCore.Title.Models;

namespace Members.Payments
{
    public class Payment : ContentPart
    {
        public NumericField Amount { get; set; }
        public TextField PayerName { get; set; }
        public TextField Address { get; set; }
        public TextField ReferenceNr { get; set; }
        public TextField PaymentRef { get; set; }
        public DateField Date { get; set; }
        public ContentPickerField Person { get; set; }
        public string BankContentItemId { get; set; }
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
                .Draftable()
                .WithPart("Payment", part => part
                    .WithPosition("0")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("1")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedDisabled,
                        Pattern = "{{ ContentItem.Content.Payment.PayerName.Text }} - {{ ContentItem.Content.Payment.Amount.Value | format_number: \"C\"  }} - {{ ContentItem.Content.Payment.Date.Value | date: \"%D\" }}",
                    })
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
                        Scale=2
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
                .WithField("Date", field => field
                    .OfType("DateField")
                    .WithDisplayName("Datum plaćanja")
                )
                .WithField("Person", field => field
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
