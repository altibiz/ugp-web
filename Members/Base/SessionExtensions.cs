using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Lists.Models;
using System.Threading.Tasks;
using YesSql;

namespace Members.Base
{
    public static class SessionExtensions
    {
        public async static Task<ContentItem> GetItemById(this ISession session,string contentItemId)
        {
            return await session.Query<ContentItem, ContentItemIndex>(x => x.ContentItemId == contentItemId).FirstOrDefaultAsync();
        }

        public async static Task<ContentItem> GetListParent(this ISession session,ContentItem childItem)
        {
            return await session.GetItemById(childItem.As<ContainedPart>()?.ListContentItemId);
        }
    }
}
