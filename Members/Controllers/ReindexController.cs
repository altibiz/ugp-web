using Members.Base;
using Members.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YesSql;

namespace Members.Controllers
{
    [ApiController]
    [Route("api/reindex")]
    [Authorize(Roles ="Administrator")]
    public class ReindexController : Controller
    {
        private readonly ISession _session;

        public ReindexController(ISession session)
        {

            _session = session;
        }

        [HttpGet("paymentbyday")]
        public async Task<IActionResult> PaymentByDay()
        {
            await _session.RefreshReduceIndex(new PaymentByDayIndexProvider(), "Payment");
            return Ok(await _session.QueryIndex<PaymentByDayIndex>().ListAsync());
        }

        [HttpGet("payment")]
        public async Task<IActionResult> Payment()
        {
            await _session.RefreshMapIndex(new PaymentIndexProvider(), "Payment");
            return Ok(await _session.QueryIndex<PaymentIndex>().Take(100).ListAsync());
        }

    }
}
