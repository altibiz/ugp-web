using Members.Base;
using Members.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ReindexController> _logger;

        public ReindexController(ISession session,ILogger<ReindexController> logger)
        {

            _session = session;
            _logger = logger;
        }

        [HttpGet("paymentbyday")]
        public async Task<IActionResult> PaymentByDay()
        {
            await _session.RefreshReduceIndex(new PaymentByDayIndexProvider(), "Payment",logger:_logger) ;
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
