using Domain.DTO.StockAnalysis;
using Domain.DTO.WebScraper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.CompositionAndRecommendationService
{
    public interface ICompositionAndRecommendationService
    {
        Task<CompositionAndRecommendation> GetCompositionAndRecommendation(ulong userId);
        Task<Dictionary<string, ScraperAnalysis>> GetAnalystRecommendation(IEnumerable<string> tickers);
    }
}
