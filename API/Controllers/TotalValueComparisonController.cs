using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.TotalValueComparisonService;
using Domain.DTO;
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

        [HttpGet("GetTotalValueComparison/{userId}/{from}/{to}")]
        public async Task<TotalValueComparisonToIndex> GetTotalValueComparison(ulong userId, DateTime from, DateTime to)
        {
            try
            {
                var indexTickers = IndexTickers.GetAllowedIndexTickers();
                var totalGainOrLoss = await _totalValueComparisonService.GetTotalValueComparison(userId, indexTickers, from, to);

                return totalGainOrLoss;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
