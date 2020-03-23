using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.StockQuoteService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockQuoteController : ControllerBase
    {
        private readonly IStockQuoteService _stockQuoteService;
        public StockQuoteController(IStockQuoteService stockQuoteService)
        {
            _stockQuoteService = stockQuoteService;
        }

        // GET: api/StockQuote
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _stockQuoteService.GetLatestQuoteForStocks(new List<string>()
            {
                "MSFT",
                //"AAPL"
            });
            return new string[] { "value1", "value2" };
        }
    }
}
