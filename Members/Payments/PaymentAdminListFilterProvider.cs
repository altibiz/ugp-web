using Members.Payments;
using OrchardCore.ContentManagement;
using OrchardCore.Contents.Services;
using YesSql.Filters.Query;

namespace Members.Payments
{
    public class PaymentAdminListFilterProvider : IContentsAdminListFilterProvider
    {
        public void Build(QueryEngineBuilder<ContentItem> builder)
        {
            builder
                .WithNamedTerm("payout", builder => builder
                    .OneCondition((val, query) =>
                    {
                        if (!string.IsNullOrEmpty(val))
                        {
                            if (val == "true")
                                query.With<PaymentIndex>(i => i.IsPayout);
                            else if (val == "false")
                                query.With<PaymentIndex>(x => !x.IsPayout);
                        }

                        return query;
                    })
                );
        }
    }
}
