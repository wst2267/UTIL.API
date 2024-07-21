using Microsoft.AspNetCore.Mvc;
using util.API.Models;
using util.API.Models.Request;
using util.API.Service;

namespace util.API.Controllers
{
    [Route("api/ledger")]
    [ApiController]
    public class LedgerController : ControllerBase
    {
        private readonly ILedgerService _ledgerService;
        public LedgerController(ILedgerService ledgerService)
        {
            _ledgerService = ledgerService;
        }

        [HttpGet]
        [Route("getledger")]
        [ProducesResponseType(typeof(LedgerModel), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetLedger(string userName)
        { 
            try
            { 
                var ledger = await _ledgerService.GetLedgerAsync(userName);
                return StatusCode(StatusCodes.Status200OK, ledger);
            } 
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("upsert/{userName}")]
        public async Task<IActionResult> UpsertLedger(string userName, InsertLedgerRequest request)
        {
            try
            {
                await _ledgerService.UpsertLedgerAsync(userName, request);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
