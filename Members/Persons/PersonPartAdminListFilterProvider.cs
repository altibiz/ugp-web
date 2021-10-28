using OrchardCore.ContentManagement;
using OrchardCore.Contents.Services;
using YesSql.Filters.Query;

namespace Members.Persons
{
    public class PersonPartAdminListFilterProvider: IContentsAdminListFilterProvider
    {
        public void Build(QueryEngineBuilder<ContentItem> builder)
        {
            builder
                .WithDefaultTerm("oib", builder => builder
                    .OneCondition((val, query) =>
                    {
                        if (!string.IsNullOrEmpty(val))
                        {
                            query.With<PersonPartIndex>(i => i.Oib == val);
                        }

                        return query;
                    })
                );
        }
    }
}
