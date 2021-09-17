using OrchardCore.ContentManagement;
using OrchardCore.Contents.Services;
using OrchardCore.Contents.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesSql.Filters.Query;

namespace Members.Persons
{
    public class PersonPartAdminListFilterProvider: IContentsAdminListFilterProvider
    {
        public void Build(QueryEngineBuilder<ContentItem> builder)
        {
            builder
                .WithNamedTerm("oib", builder => builder
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
