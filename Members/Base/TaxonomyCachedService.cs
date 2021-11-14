using OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Taxonomies.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Members.Base
{
    public class TaxonomyCachedService
    {
        private IOrchardHelper _helper;

        private Dictionary<(string, string), ContentItem> _cached = new Dictionary<(string, string), ContentItem>();

        public TaxonomyCachedService(IOrchardHelper helper)
        {
            _helper = helper;
        }
        public async Task<List<ContentItem>> GetTaxonomyTerms(TaxonomyField field)
        {
            var res = new List<ContentItem>();
            foreach (var trm in field?.TermContentItemIds ?? Array.Empty<string>())
            {
                if (!_cached.TryGetValue((field.TaxonomyContentItemId, trm), out var contentItem))
                    _cached[(field.TaxonomyContentItemId, trm)] = contentItem = await _helper.GetTaxonomyTermAsync(field.TaxonomyContentItemId, trm);
                res.Add(contentItem);
            }
            return res;
        }

        public async Task<ContentItem> GetFirstTerm(TaxonomyField field)
        {
            var res = new List<ContentItem>();
            foreach (var trm in field?.TermContentItemIds ?? Array.Empty<string>())
            {
                if (!_cached.TryGetValue((field.TaxonomyContentItemId, trm), out var contentItem))
                    _cached[(field.TaxonomyContentItemId, trm)] = contentItem = await _helper.GetTaxonomyTermAsync(field.TaxonomyContentItemId, trm);
                res.Add(contentItem);
            }
            return res.FirstOrDefault(); ;
        }
    }
}
