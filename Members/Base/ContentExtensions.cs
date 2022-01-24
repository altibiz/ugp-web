using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Lists.Models;
using System.Linq;

namespace Members.Utils
{
    public static class ContentExtensions
    {
        public static TSetting GetSettings<TSetting>(this IContentDefinitionManager _cdm, ContentPart part)
    where TSetting : new()
        {
            var contentTypeDefinition = _cdm.GetTypeDefinition(part.ContentItem.ContentType);
            var contentTypePartDefinition = contentTypeDefinition.Parts.FirstOrDefault(x => string.Equals(x.PartDefinition.Name, part.GetType().Name));
            return contentTypePartDefinition.GetSettings<TSetting>();
        }

        public static void AddToList(this ContentItem parent, ContentItem child)
        {
            child.Weld<ContainedPart>();
            child.Alter<ContainedPart>(x => x.ListContentItemId = parent.ContentItemId);
        }

        //Returns content part only if the item is not deleted (not latest or published)
        public static T AsReal<T>(this ContentItem contentItem) where T : ContentPart
        {
            if (!contentItem.Latest && !contentItem.Published) return null;
            return contentItem.As<T>();
        }
    }
}
