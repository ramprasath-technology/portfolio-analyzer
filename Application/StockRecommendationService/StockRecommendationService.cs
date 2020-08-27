using Application.WebScraperService;
using Domain.DTO.WebScraper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockRecommendationService
{
    public class StockRecommendationService : IStockRecommendationService
    {
        private readonly IWebScraperService _webScraperService;
        public StockRecommendationService(IWebScraperService webScraperService)
        {
            _webScraperService = webScraperService;
        }

        public async Task<IEnumerable<ScraperAnalysis>> GetAnalysisAndRecommendation(IEnumerable<string> tickers)
        {
            var data = await _webScraperService.ScrapCNNPage(tickers);

            return data;
        }
    }
}
