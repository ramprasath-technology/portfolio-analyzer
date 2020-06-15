using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.StockTotalValueService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTotalValueController : ControllerBase
    {
        private readonly IStockTotalValueService _stockTotalValueService;

        public StockTotalValueController(IStockTotalValueService stockTotalValueService)
        {
            _stockTotalValueService = stockTotalValueService;
        }

        [HttpGet("GetTotalHoldingValue/{userId}")]
        public async Task<IActionResult> GetTotalHoldingValue(ulong userId)
        {
            try
            {
                var holdingValue = await _stockTotalValueService.GetTotalValueForUserHolding(userId);
                return Ok(holdingValue);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }
    }
}