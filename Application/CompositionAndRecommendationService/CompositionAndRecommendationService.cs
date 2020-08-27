using Application.PortfolioCompositionService;
using Application.StockHoldingService;
using Application.StockRecommendationService;
using Application.WebScraperService;
using Domain.DTO.StockAnalysis;
using Domain.DTO.WebScraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CompositionAndRecommendationService
{
    public class CompositionAndRecommendationService : ICompositionAndRecommendationService
    {
        private readonly IPortfolioCompositionService _portfolioCompositionService;
        private readonly IStockRecommendationService _stockRecommendationService;
        private readonly IStockHoldingService _holdingsService;

        public CompositionAndRecommendationService(IPortfolioCompositionService portfolioCompositionService,
            IStockRecommendationService stockRecommendationService,
            IStockHoldingService holdingService)
        {
            _portfolioCompositionService = portfolioCompositionService;
            _stockRecommendationService = stockRecommendationService;
            _holdingsService = holdingService;
        }

        public async Task<CompositionAndRecommendation> GetCompositionAndRecommendation(ulong userId)
        {
            var compositionAndRecommendation = new CompositionAndRecommendation();
            var holdings = await _holdingsService.GetAllHoldingsForUserWithStockDetails(userId);
            var tickers = holdings.Select(x => x.Stock.Ticker).Distinct();
            var recommendationTask = _stockRecommendationService.GetAnalysisAndRecommendation(tickers);
            var portfolioTask = _portfolioCompositionService.GetPortfolioComposition(userId);
            var portfolio = await portfolioTask;
            var recommendation = await recommendationTask;
            MapCompositionAndRecommendation(compositionAndRecommendation, portfolio, recommendation);

            return compositionAndRecommendation;
        }

        private void MapCompositionAndRecommendation (CompositionAndRecommendation compositionAndRecommendation,
            PortfolioComposition portfolioComposition,
            IEnumerable<ScraperAnalysis> scraperAnalyses)
        {
            var analysesMap = MapAnalyses(scraperAnalyses);
            compositionAndRecommendation.TotalCost = portfolioComposition.TotalCost;
            compositionAndRecommendation.TotalInvestmentValue = portfolioComposition.TotalInvestmentValue;
            var compositionAndRecommendationMap = new Dictionary<string, Tuple<IndividualStockWeightage, ScraperAnalysis>>();

            foreach (var stockWeightage in portfolioComposition.IndividualStockWeightages)
            {
                if (!compositionAndRecommendationMap.ContainsKey(stockWeightage.Ticker) && analysesMap.ContainsKey(stockWeightage.Ticker))
                {
                    var portfolioAndRecommendation = new Tuple<IndividualStockWeightage, ScraperAnalysis>(stockWeightage, analysesMap[stockWeightage.Ticker]);
                    compositionAndRecommendationMap[stockWeightage.Ticker] = portfolioAndRecommendation;
                }
            }
            compositionAndRecommendation.Analyses = compositionAndRecommendationMap;
        }

        private Dictionary<string, ScraperAnalysis> MapAnalyses(IEnumerable<ScraperAnalysis> scraperAnalyses)
        {
            var analysisMap = new Dictionary<string, ScraperAnalysis>();

            foreach (var analysis in scraperAnalyses)
            {
                if (!string.IsNullOrEmpty(analysis.Ticker))
                {
                    analysisMap[analysis.Ticker] = analysis;
                }
            }

            return analysisMap;
        }
    }
}
