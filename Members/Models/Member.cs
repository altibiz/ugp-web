using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.Models
{
    public class Member:ContentPart
    {
        public TextField Oib { get; set; }
        public TextField Surname { get; set; }

        public UserPickerField User { get; set; }
    }
}
