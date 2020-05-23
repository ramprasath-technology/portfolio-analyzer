using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.StockAndPurchaseService;
using Application.StockHoldingService;
using Application.StockPurchaseService;
using Application.StockService;
using Domain.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockPurchaseController : ControllerBase
    {
        private readonly IStockAndPurchaseService _stockAndPurchaseService;
        private readonly IStockHoldingService _stockHoldingService;
        public StockPurchaseController(IStockAndPurchaseService stockAndPurchaseService, 
            IStockHoldingService stockHoldingService)
        {
            _stockAndPurchaseService = stockAndPurchaseService;
            _stockHoldingService = stockHoldingService;
        }

        // POST: api/StockPurchase
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StockPurchase purchaseData)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest();
                }

                await _stockAndPurchaseService.AddStockAndPurchaseInfo(purchaseData);
                await _stockHoldingService.AddPurchaseToHoldings(purchaseData.UserId, purchaseData.Purchase);

                return Ok();
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }          
        }

        // PUT: api/StockPurchase/5
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
