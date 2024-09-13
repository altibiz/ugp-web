using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Title.Models;
using System.Threading.Tasks;

namespace Members.Payments
{
    public class BankStatPart : ContentPart
    {
        public string StatementJson { get; set; }
        public DateField Date { get; set; }
    }

    public static class BankStatementMigrations
    {
        public static async Task CreateBankStatement(this IContentDefinitionManager _contentDefinitionManager)
        {
            await _contentDefinitionManager.AlterTypeDefinitionAsync("BankStatement", type => type
                .DisplayedAs("Izvod")
                .Listable()
                .Securable()
                .Creatable()
                .WithPart(nameof(BankStatPart), part => part
                    .WithPosition("0")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("1")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedDisabled,
                        Pattern = "{{ ContentItem.Content.BankStatPart.Date.Value | date: \"%D\" }}",
                    })
                )
            );
            await _contentDefinitionManager.AlterPartDefinitionAsync(nameof(BankStatPart), part => part
                .WithField("Date", field => field
                    .OfType("DateField")
                    .WithDisplayName("Datum")
                    .WithPosition("0"))) ;
        }
    }
}