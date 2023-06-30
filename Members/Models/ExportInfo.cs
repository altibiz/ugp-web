using Members.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.Models
{
    public class ExportInfo
    {
        public DateTime? LastSave { get; set; }
        public List<Term> CountyList { get; set; }
        public List<Term> ActivityList { get; set; }
    }
}
