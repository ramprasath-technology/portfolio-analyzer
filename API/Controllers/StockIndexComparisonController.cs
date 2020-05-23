using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.StockIndexComparisonService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockIndexComparisonController : ControllerBase
    {
        private readonly IStockIndexComparisonService _stockIndexComparisonService;

        public StockIndexComparisonController(IStockIndexComparisonService stockIndexComparisonService)
        {
            _stockIndexComparisonService = stockIndexComparisonService;
        }

        // GET: api/<controller>
        [HttpGet("GetStockIndexComparisonForUserHoldings/{userId}")]
        public async Task<IActionResult> GetStockIndexComparisonForUserHoldings(ulong userId)
        {
            try
            {
                var tickers = new List<string>() { "ONEQ", "VOO", "QQQ" };
                var stockIndexComparison = await _stockIndexComparisonService.GetComparisonWithIndex(userId, tickers);
                return Ok(stockIndexComparison);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

     
    }
}
