using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.DisplayManagement.Notify;
using Members.Core;
using System.Collections.Generic;
using Members.Payments;
using System.Linq;

namespace Members.Pages
{
    public class MyDonationsModel : PageModel
    {
        private readonly IHtmlLocalizer H;
        private readonly MemberService _memberService;
        private readonly INotifier _notifier;

        public List<Payment> Payments { get; set; }
        public MyDonationsModel(MemberService mService, IHtmlLocalizer<CreateMemberModel> htmlLocalizer, INotifier notifier)
        {
            _notifier = notifier;
            H = htmlLocalizer;
            _memberService = mService;
        }
                
        public async Task OnGetAsync(string companyId)
        {
            Payments = await _memberService.GetUserPayments().ToListAsync();
        }
    }
}