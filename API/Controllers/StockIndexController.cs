using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.StockIndexTickerService;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockIndexController : ControllerBase
    {
        private IStockIndexTickerService _stockIndexService;
        public StockIndexController(IStockIndexTickerService stockIndexService)
        {
            _stockIndexService = stockIndexService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StockIndexTicker stockIndexTicker)
        {
            try
            {
                var tickerExists = await _stockIndexService.CheckIfStockTickerExists(stockIndexTicker.Ticker);
                if (!tickerExists)
                {
                    var tickerId = await _stockIndexService.AddStockIndex(stockIndexTicker);
                    return Ok(tickerId);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}