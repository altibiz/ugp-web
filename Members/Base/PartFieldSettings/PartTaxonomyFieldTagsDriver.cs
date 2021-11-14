using Members.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Taxonomies.Drivers;
using OrchardCore.Taxonomies.Fields;
using OrchardCore.Taxonomies.Models;
using OrchardCore.Taxonomies.Settings;
using OrchardCore.Taxonomies.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Members.PartFieldSettings
{
    public class PartTaxonomyFieldTagsDriver : TaxonomyFieldTagsDisplayDriver
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private IHttpContextAccessor _httpCA;
        private IContentManager _contentManager;

        public PartTaxonomyFieldTagsDriver(IContentManager cm, IStringLocalizer<TaxonomyFieldTagsDisplayDriver> localizer, IHttpContextAccessor httpContextAccessor) : base(cm, localizer)
        {
            _httpCA = httpContextAccessor;
            _contentManager = cm;
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

                    model.TagTermEntries = JsonConvert.SerializeObject(tagTermEntries, SerializerSettings);
                }

                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = fieldDef;
            });

        }

        public override async Task<IDisplayResult> UpdateAsync(TaxonomyField field, IUpdateModel updater, UpdateFieldEditorContext context)
        {
            var fieldDef = DriverService.GetFieldDef(context, AdminAttribute.IsApplied(_httpCA.HttpContext));
            if (fieldDef == null) return null;
            if (fieldDef.Editor() == "Disabled") return Edit(field, context);
            return await base.UpdateAsync(field, updater, context);
        }
    }
}
