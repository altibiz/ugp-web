using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Contents.Services;
using System.Threading.Tasks;
using YesSql.Filters.Query;
using YesSql.Services;

namespace Members.Persons
{
    public class PersonPartAdminListFilterProvider : IContentsAdminListFilterProvider
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
                )
                .WithDefaultTerm("text", builder => builder
                        .ManyCondition(
                            async (val, query, ctx) =>
                            {
                                var context = (ContentQueryContext)ctx;
                                var accessr = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
                                if(accessr.HttpContext.Request.RouteValues.TryGetValue("contentTypeId", out var selectedContentType));
                                if(selectedContentType?.ToString()=="Member"|| selectedContentType?.ToString() == "Company")
                                {
                                    return query.With<PersonPartIndex>(x => x.Oib == val || x.LegalName.Contains(val));
                                }
                                else
                                return query.With<ContentItemIndex>(x => x.DisplayText.Contains(val));
                            },
                            async (val, query, ctx) =>
                            {
                                return query.With<ContentItemIndex>(x => x.DisplayText.IsNotIn<ContentItemIndex>(s => s.DisplayText, w => w.DisplayText.Contains(val)));
                            }
                        )
                    );
        }
    }
}
