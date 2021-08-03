using OrchardCore.ContentManagement;

namespace Members.Models
{
    public interface IHideField
    {
        public bool IsFieldHidden(string propertyName, ContentPart part);
    }
}