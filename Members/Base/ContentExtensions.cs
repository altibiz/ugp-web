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
        public static async Task<TSetting> GetSettings<TSetting>(this IContentDefinitionManager _cdm, ContentPart part)
    where TSetting : new()
        {
            var contentTypeDefinition = await _cdm.GetTypeDefinitionAsync(part.ContentItem.ContentType);
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
                    prop.SetValue(part, part.GetOrCreate<TextField>(prop.Name));
                if (prop.PropertyType == typeof(NumericField) && prop.GetValue(part) == null)
                    prop.SetValue(part, part.GetOrCreate<NumericField>(prop.Name));
                if (prop.PropertyType == typeof(DateField) && prop.GetValue(part) == null)
                    prop.SetValue(part, part.GetOrCreate<DateField>(prop.Name));
                if (prop.PropertyType == typeof(DateField) && prop.GetValue(part) == null)
                    prop.SetValue(part, part.GetOrCreate<DateField>(prop.Name));
                if (prop.PropertyType == typeof(TaxonomyField) && prop.GetValue(part) == null)
                    prop.SetValue(part, part.GetOrCreate<TaxonomyField>(prop.Name));
                if (prop.PropertyType == typeof(ContentPickerField) && prop.GetValue(part) == null)
                    prop.SetValue(part, part.GetOrCreate<ContentPickerField>(prop.Name));
                if (prop.PropertyType == typeof(UserPickerField) && prop.GetValue(part) == null)
                    prop.SetValue(part, part.GetOrCreate<UserPickerField>(prop.Name));
                if (prop.PropertyType == typeof(BooleanField) && prop.GetValue(part) == null)
                    prop.SetValue(part, part.GetOrCreate<BooleanField>(prop.Name));
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

        public static IEnumerable<T> AsParts<T>(this IEnumerable<ContentItem> items) where T : ContentPart
        {
            return items.Select(x => x.As<T>());
        }
    }
}
