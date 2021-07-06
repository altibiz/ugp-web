using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.ViewModels
{
    public class UserLandingPageModel
    {
        [BindNever]
        public string Title { get; set; }

        [BindNever]
        public dynamic ContentItem { get; set; }
    }
}
