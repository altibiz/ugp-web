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
        [BindProperty]
        public string PersonId { get; set; }
        public List<ContentItem> PersonList { get; set; }
        public bool IsGuest { get; set; }

        public DonateModel(MemberService mService, IHtmlLocalizer<CreateMemberModel> htmlLocalizer, INotifier notifier)
        {
            _notifier = notifier;
            H = htmlLocalizer;
            _memberService = mService;
            IsGuest = true;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ContentItem ci = await _memberService.GetUserMember();
            if (ci != null)
            {
                await SetPersonList();

                IsGuest = false;
                LegalName = ci.Content.PersonPart.LegalName.ToString();
                Amount = "1000";
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
        public async Task<IActionResult> OnPost(string amount=null)
        {
            ContentItem person = await _memberService.GetContentItemById(PersonId);
            IsGuest = false;
            LegalName = person.Content.PersonPart.LegalName.ToString();
            Oib = person.Content.PersonPart.Oib.Text;
            Email = person.Content.PersonPart.Email.Text;

            await SetPersonList();

            return Page();
        }
        public async Task SetPersonList()
        {
            PersonList = new List<ContentItem>();

            ContentItem ci = await _memberService.GetUserMember();
            if (ci != null)
            {
                PersonList.Add(ci);

                var companies = await _memberService.GetUserCompanies();
                foreach (ContentItem item in companies)
                {
                    PersonList.Add(item);
                }
            }
        }

        public async Task<IActionResult> OnPostGenerateQrFromFormAsync()
        {
            return Page();
        }
    }

}
