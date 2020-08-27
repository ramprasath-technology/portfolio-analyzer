using Domain.DTO.WebScraper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockRecommendationService
{
    public interface IStockRecommendationService
    {
        Task<IEnumerable<ScraperAnalysis>> GetAnalysisAndRecommendation(IEnumerable<string> tickers);
    }
}
