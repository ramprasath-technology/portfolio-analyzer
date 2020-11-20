using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.StockSplitService;
using Domain.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockSplitController : ControllerBase
    {
        private readonly IStockSplitService _stockSplitService;
        public StockSplitController(IStockSplitService stockSplitService)
        {
            _stockSplitService = stockSplitService;
        }

        [HttpPost("{userId}")]
        public async Task PerformStockSplit(ulong userId, [FromBody] StockSplit splitDetails)
        {
            await _stockSplitService.PerformStockSplit(userId, splitDetails);
        }
    }
}