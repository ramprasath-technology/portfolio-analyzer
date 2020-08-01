using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.StockIndexValueService;
using Domain.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockIndexValueController : ControllerBase
    {
        private readonly IStockIndexValueService _stockIndexValueService;
        
        public StockIndexValueController(IStockIndexValueService stockIndexValueService)
        {
            _stockIndexValueService = stockIndexValueService;
        }

        [HttpPost]
        public async Task<IActionResult> AddStockIndexValues([FromBody] StockIndexValueInputs indexValueInputs)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                await _stockIndexValueService.AddIndexValues(indexValueInputs);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetIndexValues/{userId}")]
        public async Task<IActionResult> GetIndexValues(ulong userId)
        {
            var index = IndexTickers.GetAllowedIndexTickers();
            var date = new List<DateTime>() { DateTime.Now.AddDays(-30).Date };
            await _stockIndexValueService.GetPricesForGivenIndexTickersAndDates(userId, index, date);

            return Ok();
        }    
    }
}
