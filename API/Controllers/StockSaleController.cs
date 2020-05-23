using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.StockSaleService;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockSaleController : ControllerBase
    {
        private readonly IStockSaleService _stockSaleService;

        public StockSaleController(IStockSaleService stockSaleService)
        {
            _stockSaleService = stockSaleService;
        }

        // POST: api/StockSale
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Sale sale)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid inputs");
                }

                await _stockSaleService.AddSale(sale.UserId, sale);

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

    }
}
