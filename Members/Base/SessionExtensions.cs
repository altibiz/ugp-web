using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Lists.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YesSql;
using YesSql.Indexes;

namespace Members.Base
{
    public static class SessionExtensions
    {
        public async static Task<ContentItem> GetItemById(this ISession session, string contentItemId)
        {
            return await session.Query<ContentItem, ContentItemIndex>(x => x.ContentItemId == contentItemId).FirstOrDefaultAsync();
        }

        public async static Task<ContentItem> GetListItemParent(this ISession session, ContentItem childItem)
        {
            return await session.GetItemById(childItem.GetParentItemId());
        }

        public static string GetParentItemId(this ContentItem childItem)
        {
            return childItem.As<ContainedPart>()?.ListContentItemId;
        }

        public async static Task<ContentItem> FirstOrDefaultAsync<TIndex>(this ISession session, IContentManager manager, Expression<Func<TIndex, bool>> query) where TIndex : class, IIndex
        {
            return await session.Query<ContentItem, TIndex>(query).FirstOrDefaultAsync(manager);
        }
    }
}
