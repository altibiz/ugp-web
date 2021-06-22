using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.ViewModels
{
    public class MemberListContentsViewModel
    {
        public MemberListContentsViewModel()
        {

        }

        public int? Page { get; set; }
        [BindNever]
        public List<dynamic> ContentItems { get; set; }
        [BindNever]
        public dynamic Pager { get; set; }
    }
}
