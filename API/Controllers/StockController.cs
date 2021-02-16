using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.StockService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpPut("UpdateCompanyProfile")]
        public async Task<IActionResult> UpdateCompanyProfile()
        {
            try
            {
                await _stockService.UpdateCompanyProfile();
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}