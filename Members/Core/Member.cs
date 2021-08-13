using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Taxonomies.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.Core
{
    public class Member:ContentPart
    {
        public UserPickerField User { get; set; }

        public DateField DateOfBirth { get; set; }

        public TaxonomyField Sex { get; set; }

        public TaxonomyField Activity { get; set; }

        public TextField Skills { get; set; }
    }


}
