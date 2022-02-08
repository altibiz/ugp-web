using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Members.Core;
using Members.Payments;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YesSql;

namespace Members.Pages
{
    public class TransparencyModel : PageModel
    {
        private readonly ISession _session;
        public IEnumerable<PaymentByDayIndex> PaymentsByDay { get; set; } = new List<PaymentByDayIndex>();

        public decimal TotalIncome { get => PaymentsByDay.Sum(x => x.PayIn); }

        public decimal TotalExpense { get => PaymentsByDay.Sum(x => x.PayOut); }

        public decimal PayIns { get => PaymentsByDay.Sum(x => x.CountIn); }

        public decimal PayOuts { get => PaymentsByDay.Sum(x => x.CountOut); }

        public decimal AverageIn { get => PayIns > 0 ? TotalIncome / PayIns : 0; }

        public decimal AverageOut { get => PayOuts > 0 ? TotalExpense / PayOuts : 0; }

        public TransparencyModel(ISession session, MemberService mService)
        {
            _session = session;
        }

        public async Task OnGetAsync()
        {
           PaymentsByDay  = await _session.QueryIndex<PaymentByDayIndex>().ListAsync();
        }
    }
}
