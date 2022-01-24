using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.DisplayManagement.Notify;
using System.Collections.Generic;
using Members.Payments;
using System.Linq;

namespace Members.Pages
{
    public class MyDonationsModel : PageModel
    {
        private readonly PaymentService _pService;

        public List<Payment> Payments { get; set; }
        public MyDonationsModel(PaymentService pService, IHtmlLocalizer<CreateMemberModel> htmlLocalizer, INotifier notifier)
        {
            _pService = pService;
        }
                
        public async Task OnGetAsync(string companyId)
        {
            Payments = await _pService.GetUserPayments().ToListAsync();
        }
    }
}