using OrchardCore.Taxonomies.ViewModels;

namespace UgpTheme.Drivers
{
    public class EditTxnFieldViewModel : EditTaxonomyFieldViewModel
    {
        public string TaxonomyContentItemId { get; set; }
    }

    public class EditTagTxnFieldViewModel : EditTagTaxonomyFieldViewModel
    {
        public string TaxonomyContentItemId { get; set; }
    }
}
