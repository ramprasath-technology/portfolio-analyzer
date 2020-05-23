using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.StockHoldingService;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockHoldingController : ControllerBase
    {
        private readonly IStockHoldingService _stockHoldingService;

        public StockHoldingController(IStockHoldingService stockHoldingService)
        {
            _stockHoldingService = stockHoldingService;
        }


        // GET: api/StockHolding
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            try
            {
                var purchase = new Purchase();
                purchase.PurchaseId = 1;
                purchase.StockId = 1;
                purchase.Price = 100;
                purchase.Quantity = 5;

                await _stockHoldingService.AddPurchaseToHoldings(1, purchase);
                return new string[] { "value1", "value2" };
            }
            catch(Exception e)
            {
                throw e;
            }
        }

    }
}
