using Members.Models;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.DisplayManagement.Views;

namespace Members.Persons
{
    public class MembersTextFieldDriver : ContentFieldDisplayDriver<TextField>
    {
        private readonly IStringLocalizer S;

        public MembersTextFieldDriver(IStringLocalizer<MembersTextFieldDriver> localizer)
        {
            S = localizer;
        }

        public override IDisplayResult Edit(TextField field, BuildFieldEditorContext context)
        {
            var partSettings = context.TypePartDefinition.GetSettings<PersonPartSettings>();
            if (partSettings != null)
            {
                if (partSettings.IsFieldHidden(context.PartFieldDefinition.Name))
                    return null;
                var textset = context.PartFieldDefinition.GetSettings<ContentPartFieldSettings>();
                textset.DisplayName = partSettings.GetFieldLabel(context.PartFieldDefinition.Name, textset.DisplayName);
            }
            return Initialize<EditTextFieldViewModel>(GetEditorShapeType(context), model =>
            {
                model.Text = field.Text;
                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = context.PartFieldDefinition;
            });
        }
    }
}
