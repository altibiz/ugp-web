using Members.Base;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Lists.Models;
using OrchardCore.Taxonomies.Fields;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public static T InitFields<T>(this T part) where T : ContentPart
        {
            if (part == null) return part;
            foreach (var prop in part.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(TextField) && prop.GetValue(part) == null)
                    prop.SetValue(part, new TextField { ContentItem = part.ContentItem });
                if (prop.PropertyType == typeof(NumericField) && prop.GetValue(part) == null)
                    prop.SetValue(part, new NumericField { ContentItem = part.ContentItem });
                if (prop.PropertyType == typeof(DateField) && prop.GetValue(part) == null)
                    prop.SetValue(part, new DateField { ContentItem = part.ContentItem });
                if (prop.PropertyType == typeof(DateField) && prop.GetValue(part) == null)
                    prop.SetValue(part, new DateField { ContentItem = part.ContentItem });
                if (prop.PropertyType == typeof(TaxonomyField) && prop.GetValue(part) == null)
                    prop.SetValue(part, new TaxonomyField { ContentItem = part.ContentItem });
                if (prop.PropertyType == typeof(ContentPickerField) && prop.GetValue(part) == null)
                    prop.SetValue(part, new ContentPickerField { ContentItem = part.ContentItem });
                if (prop.PropertyType == typeof(UserPickerField) && prop.GetValue(part) == null)
                    prop.SetValue(part, new UserPickerField { ContentItem = part.ContentItem });
                if (prop.PropertyType == typeof(BooleanField) && prop.GetValue(part) == null)
                    prop.SetValue(part, new BooleanField { ContentItem = part.ContentItem });
            }
            return part;
        }

        public static string GetId(this ContentPickerField contentPickerField)
        {
            return contentPickerField?.ContentItemIds?.FirstOrDefault();
        }

        public static void SetId(this ContentPickerField contentPickerField, string value)
        {
            contentPickerField.ContentItemIds = new[] { value };
        }

        public static string GetId(this TaxonomyField taxonomyField)
        {
            return taxonomyField.TermContentItemIds?.FirstOrDefault();
        }

        public static void SetId(this TaxonomyField field, string value)
        {
            field.TermContentItemIds = new[] { value };
        }

        public static async Task<ContentItem> GetTerm(this TaxonomyField field, TaxonomyCachedService service)
        {
            return await service.GetFirstTerm(field);
        }

        public static async Task<TPart> GetTerm<TPart>(this TaxonomyField field, TaxonomyCachedService service) where TPart : ContentPart
        {
            return (await service.GetFirstTerm(field)).As<TPart>();
        }
        public static IEnumerable<T> AsParts<T>(this IEnumerable<ContentItem> items) where T : ContentPart
        {
            return items.Select(x => x.As<T>());
        }
    }
}
