using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Members.Core;
using Members.Payments;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement.Records;
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

        public decimal CompanyCount { get; set; }

        public decimal PersonCount { get; set; }
        public decimal MemberCount => PersonCount + CompanyCount;

        public decimal Employees { get; set; }
        public decimal Revenue { get; set; }
        public decimal Associates { get; set; }

        public TransparencyModel(ISession session, MemberService mService)
        {
            _session = session;
        }

        public async Task OnGetAsync()
        {

            PaymentsByDay = (await _session.QueryIndex<PaymentByDayIndex>().ListAsync()).OrderByDescending(x => x.Date);

            var conn = await _session.CreateConnectionAsync();

            var statSql = "SELECT count(oib) Count,sum(employees) Employees, sum(associates) Associates, sum(Revenue2019) Revenue FROM PersonPartIndex where persontype=@PersonType and published=1";
            dynamic stat = await conn.QuerySingleAsync(statSql, new { PersonType = "Legal" });
            Employees = stat.Employees ?? 0;
            Revenue = stat.Revenue ?? 0;
            Associates = stat.Associates ?? 0;
            CompanyCount = stat.Count ?? 0;
            stat = await conn.QuerySingleAsync(statSql, new { PersonType = "Natural" });
            PersonCount = stat.Count ?? 0;
        }
    }
}
