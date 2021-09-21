using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Members.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement.Notify;

namespace Members.Pages
{
    public class MemberConfirmationDocumentModel : PageModel
    {
        private readonly IHtmlLocalizer H;
        private readonly MemberService _memberService;
        private readonly INotifier _notifier;

        public ContentItem Member { get; set; }
        public ContentItem Company { get; set; }
        public MemberConfirmationDocumentModel(MemberService mService, IHtmlLocalizer<MemberConfirmationDocumentModel> htmlLocalizer, INotifier notifier)
        {
            _notifier = notifier;
            H = htmlLocalizer;
            _memberService = mService;
        }
        public async Task<IActionResult> OnGetAsync(string contentItemId)
        {
            Member = await _memberService.GetUserMember();
            Company = await _memberService.GetContentItemById(contentItemId);

            return Page();
        }
    }
}
