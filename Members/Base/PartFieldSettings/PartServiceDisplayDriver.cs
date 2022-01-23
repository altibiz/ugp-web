using System.Threading.Tasks;
using Members.PartFieldSettings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Mvc.ModelBinding;

namespace Members.Persons
{
    public class PartServiceDisplayDriver<TPart, TService> : ContentPartDisplayDriver<TPart> where TPart : ContentPart, new()
        where TService : IPartService<TPart>
    {

        private readonly TService _service;

        public PartServiceDisplayDriver(
            TService service
        )
        {
            _service = service;
        }

        public override IDisplayResult Edit(TPart part, BuildPartEditorContext context)
        {
            var initer = _service.GetEditModel(part, context);
            if (initer != null)
                return Initialize(GetEditorShapeType(context), initer);
            return base.Edit(part);
        }


        public override async Task<IDisplayResult> UpdateAsync(TPart model, IUpdateModel updater, UpdatePartEditorContext context)
        {
            await updater.TryUpdateModelAsync(model, Prefix);

            await foreach (var item in _service.ValidateAsync(model))
            {
                updater.ModelState.BindValidationResult(Prefix, item);
            }

            return Edit(model, context);
        }

    }
}
