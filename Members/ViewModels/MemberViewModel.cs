using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Members.ViewModels
{
    public class MemberViewModel
    {
        public MemberViewModel()
        {
        }

        [BindNever]
        public dynamic ContentItem { get; set; }

    }

}
