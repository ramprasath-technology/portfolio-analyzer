using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.TotalValueComparisonService;
using Domain.DTO.StockAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TotalValueComparisonController : ControllerBase
    {
        private readonly ITotalValueComparisonService _totalValueComparisonService;

        public TotalValueComparisonController(ITotalValueComparisonService totalValueComparisonService)
        {
            _totalValueComparisonService = totalValueComparisonService;
        }

        [HttpGet("GetTotalValueComparison/{userId}")]
        public async Task<TotalValueComparisonToIndex> GetTotalValueComparison(ulong userId)
        {
            try
            {
                var indexTickers = new List<string>() { "VOO", "ONEQ", "QQQ" };
                var totalGainOrLoss = await _totalValueComparisonService.GetTotalValueComparison(userId, indexTickers);

                return totalGainOrLoss;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
