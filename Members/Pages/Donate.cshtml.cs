using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Members.Base;
using Members.Core;
using Members.Persons;
using Members.Utils;
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
            ContentItem ci = await _memberService.GetUserMember(true);
            if (ci != null)
            {
                var pp = ci.As<PersonPart>().InitFields();
                await SetPersonList();
                IsGuest = false;
                LegalName = pp.LegalName;
                Amount = "100";
                Oib = pp.Oib.Text;
                Email = pp.Email.Text;
            }

            return Page();
        }
        public IActionResult OnGetProccessForm(string legalName=null, string oib=null, string amount=null, string email=null, string note=null)
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

        public IActionResult OnPostGenerateQrFromForm()
        {
            return Page();
        }
    }

}
