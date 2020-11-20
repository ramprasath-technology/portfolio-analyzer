using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IndividualStockComparisonService;
using Domain.DTO.StockAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndividualStockComparisonController : ControllerBase
    {
        private readonly IIndividualStockComparisonService _individualComparisonService;

        public IndividualStockComparisonController(IIndividualStockComparisonService individualComparisonService)
        {
            _individualComparisonService = individualComparisonService;
        }

        [HttpGet("GetIndividualStockComparison/{userId}")]
        public async Task<IEnumerable<IndividualStockReturn>> GetIndividualStockComparison(ulong userId)
        {
            var stockComparisons = await _individualComparisonService.GetIndividualComparisonService(userId);

            return stockComparisons;
        }
    }
}