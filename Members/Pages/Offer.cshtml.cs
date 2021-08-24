using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Members.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;

namespace Members.Pages
{
    public class OfferModel : PageModel
    {
        private readonly IHtmlLocalizer H;
        private readonly IContentManager _contentManager;
        private readonly INotifier _notifier;
        public dynamic Shape { get; set; }
        public ContentItem contentItem { get; set; }

        public OfferModel(IContentManager cManager, IHtmlLocalizer<CreateMemberModel> htmlLocalizer, INotifier notifier)
        {
            _notifier = notifier;
            H = htmlLocalizer;
            _contentManager = cManager;
        }

        public async Task OnGetAsync(string offerId)
        {
            contentItem = await _contentManager.GetAsync(offerId);
        }

    }
}
