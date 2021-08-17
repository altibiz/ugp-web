using Members.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Taxonomies.Drivers;
using OrchardCore.Taxonomies.Fields;
using System.Threading.Tasks;

namespace Members.PartFieldSettings
{
    public class PartTaxonomyFieldTagsDriver : TaxonomyFieldTagsDisplayDriver
    {
        private IHttpContextAccessor _httpCA;

        public PartTaxonomyFieldTagsDriver(IContentManager cm, IStringLocalizer<TaxonomyFieldTagsDisplayDriver> localizer, IHttpContextAccessor httpContextAccessor) : base(cm, localizer)
        {
            _httpCA = httpContextAccessor;
        }

        public override IDisplayResult Edit(TaxonomyField field, BuildFieldEditorContext context)
        {
            if (!AdminAttribute.IsApplied(_httpCA.HttpContext) && !DriverService.CheckSettings(context)) return null;
            return base.Edit(field, context);
        }

        public override async Task<IDisplayResult> UpdateAsync(TaxonomyField field, IUpdateModel updater, UpdateFieldEditorContext context)
        {
            if (!AdminAttribute.IsApplied(_httpCA.HttpContext) && !DriverService.CheckSettings(context)) return null;
            if (context.PartFieldDefinition.Editor() == "Disabled") return Edit(field, context);
            return await base.UpdateAsync(field, updater, context);
        }
    }
}
