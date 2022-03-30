using Microsoft.AspNetCore.Http;
using OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Taxonomies.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Members.Base
{
    public static class TaxonomyExtensions
    {
        private class OHelper : IOrchardHelper
        {
            public HttpContext HttpContext { get; set; }
        }

        private static AsyncLocal<Dictionary<(string, string), ContentItem>> _cachedAsync = new AsyncLocal<Dictionary<(string, string), ContentItem>>();
        private static Dictionary<(string, string), ContentItem> _cached => _cachedAsync.Value ??= new();

        private static async Task<ContentItem> GetTermAsync(this HttpContext context, string taxonomyContentItemId, string termContentItemId)
        {
            return await new OHelper { HttpContext = context}.GetTaxonomyTermAsync(taxonomyContentItemId, termContentItemId);
        }

        public static async Task<List<ContentItem>> GetTerms(this TaxonomyField field,HttpContext context)
        {
            var res = new List<ContentItem>();
            foreach (var trm in field?.TermContentItemIds ?? Array.Empty<string>())
            {
                if (!_cached.TryGetValue((field.TaxonomyContentItemId, trm), out var contentItem))
                    _cached[(field.TaxonomyContentItemId, trm)] = contentItem = await context.GetTermAsync(field.TaxonomyContentItemId, trm);
                res.Add(contentItem);
            }
            return res;
        }

        public static async Task<ContentItem> GetTerm(this TaxonomyField field,HttpContext context)
        {
            return (await field.GetTerms(context)).FirstOrDefault(); ;
        }

        public static string GetId(this TaxonomyField taxonomyField)
        {
            return taxonomyField.TermContentItemIds?.FirstOrDefault();
        }

        public static void SetId(this TaxonomyField field, string value)
        {
            field.TermContentItemIds = new[] { value };
        }
    }
}
