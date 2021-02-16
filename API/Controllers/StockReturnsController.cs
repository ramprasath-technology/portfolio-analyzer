using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.StockReturnsService;
using Domain.DTO.StockAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockReturnsController : ControllerBase
    {
        private readonly IStockReturnsService _stockReturnsService;
       public StockReturnsController(IStockReturnsService stockReturnsService)
       {
            _stockReturnsService = stockReturnsService;
       }

        [HttpGet("GetAnnualizedReturnForUserHoldings/{userId}/{minHoldingMonths?}")]
        public async Task<IActionResult> GetAnnualizedReturnForUserHoldings(ulong userId, uint? minHoldingMonths)
        {
            var annualizedReturn = await _stockReturnsService.GetAnnualizedReturnForCurrentHoldings(userId, minHoldingMonths);

            return Ok(annualizedReturn);
        }

       
    }
}
