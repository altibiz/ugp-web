using Members.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Taxonomies.Drivers;
using OrchardCore.Taxonomies.Fields;
using OrchardCore.Taxonomies.Models;
using OrchardCore.Taxonomies.Settings;
using OrchardCore.Taxonomies.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Members.PartFieldSettings
{
    public class PartTaxonomyFieldTagsDriver : ContentFieldDisplayDriver<TaxonomyField>
    {
        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private IHttpContextAccessor _httpCA;
        private IContentManager _contentManager;
        private TaxonomyFieldTagsDisplayDriver _txnDriver;

        public PartTaxonomyFieldTagsDriver(IContentManager cm, IStringLocalizer<TaxonomyFieldTagsDisplayDriver> localizer, IHttpContextAccessor httpContextAccessor)
        {
            _httpCA = httpContextAccessor;
            _contentManager = cm;
            _txnDriver = new TaxonomyFieldTagsDisplayDriver(cm, localizer);
        }

        public override IDisplayResult Display(TaxonomyField field, BuildFieldDisplayContext fieldDisplayContext)
        {
            return _txnDriver.Display(field, fieldDisplayContext);
        }

        public override IDisplayResult Edit(TaxonomyField field, BuildFieldEditorContext context)
        {
            var fieldDef = DriverService.GetFieldDef(context, AdminAttribute.IsApplied(_httpCA.HttpContext));
            if (fieldDef == null) return null;
            return Initialize<EditTagTaxonomyFieldViewModel>(GetEditorShapeType(fieldDef), async model =>
            {
                var settings = fieldDef.GetSettings<TaxonomyFieldSettings>();
                model.Taxonomy = await _contentManager.GetAsync(settings.TaxonomyContentItemId, VersionOptions.Latest);

                if (model.Taxonomy != null)
                {
                    var termEntries = new List<TermEntry>();
                    TaxonomyFieldDriverHelper.PopulateTermEntries(termEntries, field, model.Taxonomy.As<TaxonomyPart>().Terms, 0);
                    var tagTermEntries = termEntries.Select(te => new TagTermEntry
                    {
                        ContentItemId = te.ContentItemId,
                        Selected = te.Selected,
                        DisplayText = te.Term.DisplayText,
                        IsLeaf = te.IsLeaf
                    });

                    model.TagTermEntries = JsonSerializer.Serialize(tagTermEntries, options);
                }

                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = fieldDef;
            });

        }

        public override async Task<IDisplayResult> UpdateAsync(TaxonomyField field, UpdateFieldEditorContext context)
        {
            var fieldDef = DriverService.GetFieldDef(context, AdminAttribute.IsApplied(_httpCA.HttpContext));
            if (fieldDef == null) return null;
            if (fieldDef.Editor() == "Disabled") return Edit(field, context);
            return await _txnDriver.UpdateAsync(field, context);
        }
    }
}
