using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IndexAnalysisService;
using Domain.DTO.StockAnalysis;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class IndexAnalysisController : Controller
    {
        private readonly IIndexAnalysisService _indexAnalysisService;

        public IndexAnalysisController(IIndexAnalysisService indexAnalysisService)
        {
            _indexAnalysisService = indexAnalysisService;
        }

        // GET: api/<controller>
        [HttpGet("GetDailyInvestmentAnalysis/{userId}")]
        public async Task<DailyIndexInvestmentOutcome> GetDailyInvestmentAnalysis(ulong userId)
        {
            try
            {
                var tickers = new List<string>()
                {
                "VOO",
                "ONEQ",
                "QQQ"
                };

                var indexAnalysis = await _indexAnalysisService.GetReturnsForDailyInvestment(userId, tickers);
                return indexAnalysis;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        [HttpGet("GetBimonthlyInvestmentAnalysis/{userId}")]
        public async Task<DailyIndexInvestmentOutcome> GetBimonthlyInvestmentAnalysis(ulong userId)
        {
            try
            {
                var tickers = new List<string>()
                {
                "VOO",
                "ONEQ",
                "QQQ"
                };

                var indexAnalysis = await _indexAnalysisService.GetReturnsForBimonthlyInvestment(userId, tickers);
                return indexAnalysis;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
