using OrchardCore.ContentManagement;

namespace Members.PartFieldSettings
{
    public interface IFieldSettings
    {
        public bool IsFieldHidden(string propertyName, ContentPart part);

        public string GetFieldLabel(string propertyName, string defaultVale);
    }
}