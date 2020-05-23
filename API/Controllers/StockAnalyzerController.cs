using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.StockAnalyzerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockAnalyzerController : ControllerBase
    {
        private readonly IStockAnalyzerService _stockAnalyzerService;

        public StockAnalyzerController(IStockAnalyzerService stockAnalyzerService)
        {
            _stockAnalyzerService = stockAnalyzerService;
        }

        // GET: api/StockAnalyzer
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/StockAnalyzer/5
        [HttpGet("{userId}", Name = "Get")]
        public async Task<IActionResult> Get(ulong userId)
        {
            var stockSP500Mapping = await _stockAnalyzerService.GetComparisonWithSP500(userId);
            return Ok(stockSP500Mapping);
        }

        // POST: api/StockAnalyzer
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/StockAnalyzer/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
