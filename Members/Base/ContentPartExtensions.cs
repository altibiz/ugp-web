using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Taxonomies.Fields;
using System.Linq;

namespace Members.Base
{
    public static class ContentPartExtensions
    {
        public static T InitFields<T>(this T part) where T:ContentPart
        {
            foreach(var prop in part.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(TextField) && prop.GetValue(part)==null)
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
            }
            return part;
        }
    }
}
