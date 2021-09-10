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
    public class DonateModel : PageModel
    {
        private readonly IHtmlLocalizer H;
        private readonly MemberService _memberService;
        private readonly INotifier _notifier;


        [BindProperty]
        public string LegalName { get; set; }
        [BindProperty]
        public string Oib { get; set; }
        [BindProperty]
        public string Amount { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Note { get; set; }

        public bool IsGuest { get; set; }

        public DonateModel(MemberService mService, IHtmlLocalizer<CreateMemberModel> htmlLocalizer, INotifier notifier)
        {
            _notifier = notifier;
            H = htmlLocalizer;
            _memberService = mService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ContentItem ci = await _memberService.GetUserMember();
            if (Amount!= null && LegalName!=null)


            if (ci != null) {
                IsGuest = true;
                LegalName = ci.Content.PersonPart.LegalName.ToString();
                Amount = "100";
                Oib = ci.Content.PersonPart.Oib.Text;
                Email = ci.Content.PersonPart.Email.Text;

            }

            return Page();
        }
        public async Task<IActionResult> OnGetProccessFormAsync(string legalName=null, string oib=null, string amount=null, string email=null, string note=null)
        {
            IsGuest  = true;
            LegalName = legalName;
            Amount = amount;
            Oib = oib;
            Email = email;
            Note = note;

            return Page();
        }
        public async Task<IActionResult> OnPostGenerateQrFromFormAsync()
        {
            return Page();
        }
    }

}
