using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.Models;

namespace Members.PartFieldSettings
{
    public interface IFieldEditorSettings
    {
        bool IsFieldHidden(string propertyName, BuildFieldEditorContext part);

        string GetFieldLabel(string propertyName, string defaultVale);
        string GetFieldDisplayMode(string propertyName, string displayMode, BuildFieldEditorContext context);
    }
}