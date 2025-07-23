using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Members.Utils;
using Microsoft.Extensions.Localization;
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

namespace UgpTheme.Drivers;

public sealed class TxnFieldTagsDisplayDriver : ContentFieldDisplayDriver<TaxonomyField>
{
    private readonly IContentManager _contentManager;

    internal readonly IStringLocalizer S;

    public TxnFieldTagsDisplayDriver(
        IContentManager contentManager,
        IStringLocalizer<TxnFieldTagsDisplayDriver> stringLocalizer)
    {
        _contentManager = contentManager;
        S = stringLocalizer;
    }

    public override IDisplayResult Display(TaxonomyField field, BuildFieldDisplayContext context)
    {
        return Initialize<DisplayTaxonomyFieldTagsViewModel>(GetDisplayShapeType(context), model =>
        {
            model.Field = field;
            model.Part = context.ContentPart;
            model.PartFieldDefinition = context.PartFieldDefinition;
        }).Location("Detail", "Content")
        .Location("Summary", "Content");
    }

    public override IDisplayResult Edit(TaxonomyField field, BuildFieldEditorContext context)
    {
        return Initialize<EditTagTxnFieldViewModel>(GetEditorShapeType(context), async model =>
        {
            var settings = context.PartFieldDefinition.GetSettings<TaxonomyFieldSettings>();
            var taxonomyContentItemId = field.TaxonomyContentItemId ?? settings.TaxonomyContentItemId;
            model.Taxonomy = await _contentManager.GetAsync(taxonomyContentItemId, VersionOptions.Latest);

            if (model.Taxonomy != null)
            {
                var termEntries = new List<TermEntry>();

                var terms = model.Taxonomy.AsInit<TaxonomyPart>().Terms;

                // Maintain the listed order in the field, then concatenate the remaining content items.
                var sortedTerms = terms
                    .Where(x => field.TermContentItemIds.Contains(x.ContentItemId))
                    .OrderBy(x => Array.IndexOf(field.TermContentItemIds, x.ContentItemId))
                    .Concat(terms.Where(x => !field.TermContentItemIds.Contains(x.ContentItemId)))
                    .ToArray();

                TaxonomyFieldDriverHelper.PopulateTermEntries(termEntries, field, sortedTerms, 0);
                var tagTermEntries = termEntries.Select(te => new TagTermEntry
                {
                    ContentItemId = te.ContentItemId,
                    Selected = te.Selected,
                    DisplayText = te.Term.DisplayText,
                    IsLeaf = te.IsLeaf,
                });

                model.TagTermEntries = JNode.FromObject(tagTermEntries, JOptions.CamelCase).ToJsonString(JOptions.Default);
            }

            model.Field = field;
            model.Part = context.ContentPart;
            model.TaxonomyContentItemId = taxonomyContentItemId;
            model.PartFieldDefinition = context.PartFieldDefinition;
        });
    }

    public override async Task<IDisplayResult> UpdateAsync(TaxonomyField field, UpdateFieldEditorContext context)
    {
        var model = new EditTagTxnFieldViewModel();

        await context.Updater.TryUpdateModelAsync(model, Prefix, [f => f.TermContentItemIds, f => f.TaxonomyContentItemId]);

        var settings = context.PartFieldDefinition.GetSettings<TaxonomyFieldSettings>();

        field.TaxonomyContentItemId = model.TaxonomyContentItemId ?? settings.TaxonomyContentItemId;

        field.TermContentItemIds = model.TermContentItemIds == null
            ? [] : model.TermContentItemIds.Split(',', StringSplitOptions.RemoveEmptyEntries);

        if (settings.Required && field.TermContentItemIds.Length == 0)
        {
            context.Updater.ModelState.AddModelError(
                $"{Prefix}.{nameof(EditTagTaxonomyFieldViewModel.TermContentItemIds)}",
                S["A value is required for {0}.", context.PartFieldDefinition.DisplayName()]);
        }

        // Update display text for tags.
        var taxonomy = await _contentManager.GetAsync(field.TaxonomyContentItemId, VersionOptions.Latest);

        if (taxonomy == null)
        {
            return null;
        }

        var terms = new List<ContentItem>();

        foreach (var termContentItemId in field.TermContentItemIds)
        {
            var term = FindTerm(
                (JsonArray)taxonomy.Content["TaxonomyPart"]["Terms"],
                termContentItemId);

            terms.Add(term);
        }

        field.SetTagNames(terms.Select(t => t.DisplayText).ToArray());

        return Edit(field, context);
    }

    internal static ContentItem FindTerm(JsonArray termsArray, string termContentItemId)
    {
        foreach (var term in termsArray.Cast<JsonObject>())
        {
            var contentItemId = term["ContentItemId"]?.ToString();

            if (contentItemId == termContentItemId)
            {
                return term.ToObject<ContentItem>();
            }

            if (term["Terms"] is JsonArray children)
            {
                var found = FindTerm(children, termContentItemId);

                if (found != null)
                {
                    return found;
                }
            }
        }

        return null;
    }
}
