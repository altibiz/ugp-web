using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Settings;

namespace Members.Pages
{
    public class SignInModel : PageModel
    {
        private const string contentType = "Member";

        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IHtmlLocalizer H;
        private readonly dynamic New;
        private readonly INotifier _notifier;
        private readonly ISession _session;
        private readonly ISiteService _siteService;
        private readonly IUpdateModelAccessor _updateModelAccessor;


        [BindProperty]
        public string Title { get; set; } = "SignIn";

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public dynamic Password { get; set; }
        [BindProperty]
        public dynamic Pager { get; set; }

    }
}
