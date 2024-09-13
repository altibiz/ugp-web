using OrchardCore.ContentManagement;
using YesSql.Indexes;
using YesSql.Sql;
using System;
using System.Linq;
using Members.Utils;
using Members.Base;
using System.Threading.Tasks;

namespace Members.Payments
{
    public class PaymentIndex : MapIndex
    {
        public string ContentItemId { get; set; }

        public string PersonContentItemId { get; set; }

        public decimal? Amount { get; set; }

        public string PayerName { get; set; }

        public DateTime? Date { get; set; }

        public string Address { get; set; }

        public bool IsPayout { get; set; }

        public bool Published { get; set; }

        public string TransactionRef { get; set; }
    }

    public class PaymentIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<PaymentIndex>()
                .Map(contentItem =>
                {
                    var pp = contentItem.AsReal<Payment>();
                    if (pp == null) return null;
                    return new PaymentIndex
                    {
                        ContentItemId = contentItem.ContentItemId,
                        Amount = pp.Amount.Value,
                        Date = pp.Date.Value,
                        PersonContentItemId = pp.Person.ContentItemIds?.FirstOrDefault(),
                        PayerName = pp.PayerName.Text,
                        Address = pp.Address?.Text?.Length > 255 ? pp.Address?.Text?.Substring(0, 255) : pp.Address?.Text,
                        IsPayout = pp.IsPayout?.Value ?? false,
                        Published = contentItem.Published,
#pragma warning disable 0618
                        TransactionRef = pp.TransactionRef ?? pp.PaymentRef?.Text,
#pragma warning restore 0618
                    };
                });
        }
    }

    public static class PaymentIndexExtensions
    {
        public static async Task CreatePaymentIndex(this ISchemaBuilder SchemaBuilder)
        {
            await SchemaBuilder.CreateMapIndexTableAsync<PaymentIndex>(table => table
                .Column<DateTime>(nameof(PaymentIndex.Date))
                .Column<decimal?>(nameof(PaymentIndex.Amount))
                .Column<string>(nameof(PaymentIndex.ContentItemId), c => c.WithLength(50))
                .Column<string>(nameof(PaymentIndex.PersonContentItemId), c => c.WithLength(50))
                .Column<string>(nameof(PaymentIndex.PayerName), c => c.WithLength(255))
                .Column<string>(nameof(PaymentIndex.Address), c => c.WithLength(255))
            );
        }

        public static async Task AddPayoutField(this ISchemaBuilder SchemaBuilder)
        {
            await SchemaBuilder.AlterIndexTableAsync<PaymentIndex>(table =>
            {
                table.AddColumn<bool>("IsPayout");
            }
            );
        }

        public static async Task AddPaymentPublished(this ISchemaBuilder SchemaBuilder)
        {
            await SchemaBuilder.AlterIndexTableAsync<PaymentIndex>(table =>
            {
                table.AddColumn<bool>("Published");
            }
            );
            SchemaBuilder.ExecuteSql("UPDATE PaymentIndex SET Published=(SELECT Published FROM ContentItemIndex WHERE PaymentIndex.DocumentId=ContentItemIndex.DocumentId)");
        }

        public static async Task AddTransactionRef(this ISchemaBuilder SchemaBuilder)
        {
            await SchemaBuilder.AlterIndexTableAsync<PaymentIndex>(table =>
            {
                table.AddColumn<string>("TransactionRef", c => c.WithLength(50));
            });
        }
    }
}
