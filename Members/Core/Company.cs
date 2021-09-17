using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Media.Fields;
using OrchardCore.Taxonomies.Fields;

namespace Members.Core
{
    public class Company:ContentPart
    {
        public TextField AuthorizedRepresentative { get; set; }
        public NumericField Revenue2019 { get; set; }
        public NumericField EmployeeNumber { get; set; }
        public TaxonomyField OrganisationType { get; set; }
        public TaxonomyField Function { get; set; }
        public TaxonomyField Activity { get; set; }
        public NumericField PermanentAssociates { get; set; }
        public MediaField Logo { get; set; }
    }
}
