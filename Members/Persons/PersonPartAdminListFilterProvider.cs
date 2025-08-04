using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Contents.Services;
using System;
using System.Threading.Tasks;
using YesSql;
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
                .WithNamedTerm("ismember", builder => builder
                    .OneCondition((val, query) =>
                    {
                        if (!string.IsNullOrEmpty(val))
                        {
                            if (val == "true")
                                query.With<PersonPartIndex>(i => i.MembershipExpiry!=null && i.MembershipExpiry >=DateTime.Today);
                            else if (val == "false")
                                query.With<PersonPartIndex>(i => i.MembershipExpiry == null || i.MembershipExpiry < DateTime.Today);
                        }
                        return query;
                    })
                )
                .WithDefaultTerm("text", builder => builder
                        .ManyCondition(
                            async (val, query, ctx) =>
                            {
                                return await Task.Run<IQuery<ContentItem>>(() =>
                                {
                                    var context = (ContentQueryContext)ctx;
                                    var accessr = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
                                    accessr.HttpContext.Request.RouteValues.TryGetValue("contentTypeId", out var selectedContentType);
                                    if (selectedContentType?.ToString() == "Member" || selectedContentType?.ToString() == "Company")
                                    {
                                        return query.With<PersonPartIndex>(x => x.Oib == val || x.LegalName.Contains(val));
                                    }
                                    else
                                        return query.With<ContentItemIndex>(x => x.DisplayText.Contains(val));
                                });
                            },
                            async (val, query, ctx) =>
                            {
                                return await Task.Run<IQuery<ContentItem>>(() =>
                                {
                                    return query.With<ContentItemIndex>(x => x.DisplayText.IsNotIn<ContentItemIndex>(s => s.DisplayText, w => w.DisplayText.Contains(val)));
                                });
                            }
                        )
                    );
        }
    }
}
