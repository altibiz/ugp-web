using OrchardCore.ContentManagement;
using YesSql.Indexes;
using YesSql.Sql;
using System;
using System.Linq;
using Members.Utils;

namespace Members.Payments
{
    public class PaymentByDayIndex : ReduceIndex
    {
        public decimal PayOut { get; set; }

        public decimal PayIn { get; set; }
        public DateTime? Date { get; set; }
        public int CountOut { get; set; }
        public int CountIn { get; set; }
    }

    public class PaymentByDayIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<PaymentByDayIndex>()
                .Map(contentItem =>
                {
                    var pp = contentItem.AsReal<Payment>();
                    if (pp == null) return Enumerable.Empty<PaymentByDayIndex>();
                    var isPayout = pp.IsPayout?.Value ?? false;
                    var res= new PaymentByDayIndex
                    {
                        PayOut = isPayout ? pp.Amount.Value ?? 0 : 0,
                        PayIn = !isPayout ? pp.Amount.Value ?? 0 : 0,
                        Date = pp.Date.Value.Value.Date,
                        CountOut = isPayout ? 1 : 0,
                        CountIn =  isPayout ? 0 : 1
                    };
                    return new[] { res };
                }).Group(x => x.Date)
                .Reduce(g => new PaymentByDayIndex
                {
                    PayOut = g.Sum(x => x.PayOut),
                    PayIn = g.Sum(x => x.PayIn),
                    CountIn = g.Sum(x => x.CountIn),
                    CountOut = g.Sum(x => x.CountOut),
                    Date = g.Key
                }).
                Delete((ndx, map) =>
                {
                    ndx.PayOut -= map.Sum(x => x.PayOut);
                    ndx.PayIn -= map.Sum(x => x.PayIn);
                    ndx.CountIn -= map.Sum(x => x.CountIn);
                    ndx.CountOut -= map.Sum(x => x.CountOut);
                    return ndx.CountIn > 0 || ndx.CountOut > 0 ? ndx : null;
                });
        }
    }

    public static class PaymentByDayIndexExtensions
    {
        public static void CreatePaymentByDayIndex(this ISchemaBuilder SchemaBuilder)
        {
            SchemaBuilder.CreateReduceIndexTable<PaymentByDayIndex>(table => table
                .Column<DateTime>(nameof(PaymentByDayIndex.Date))
                .Column<decimal>(nameof(PaymentByDayIndex.PayIn))
                .Column<int>(nameof(PaymentByDayIndex.CountIn))
                .Column<decimal>(nameof(PaymentByDayIndex.PayOut))
                .Column<int>(nameof(PaymentByDayIndex.CountOut))
            );
        }
    }
}
