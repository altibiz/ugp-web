using System.Threading.Tasks;
using Members.Models;
using Members.Utils;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Mvc.ModelBinding;
using YesSql;

namespace Members.Persons
{
    public class PersonPartDisplayDriver : ContentPartDisplayDriver<PersonPart>
    {

        private readonly ISession _session;
        private readonly IStringLocalizer S;
        private readonly IContentDefinitionManager _cdm;

        public PersonPartDisplayDriver(
            ISession session,
            IStringLocalizer<PersonPartDisplayDriver> localizer, IContentDefinitionManager cdm
        )
        {
            _session = session;
            S = localizer;
            _cdm = cdm;
        } 

        public override IDisplayResult Edit(PersonPart part, BuildPartEditorContext context)
        {
            var res= base.Edit(part, context);
            return res;
        }

        public override async Task<IDisplayResult> UpdateAsync(PersonPart model, IUpdateModel updater, UpdatePartEditorContext context)
        {
            await updater.TryUpdateModelAsync(model, Prefix);

            await foreach (var item in model.ValidateAsync(S, _session,_cdm.GetSettings<PersonPartSettings>(model)))
            {
                updater.ModelState.BindValidationResult(Prefix, item);
            }

            return Edit(model, context);
        }

    }
}
