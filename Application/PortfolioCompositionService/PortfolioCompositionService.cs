using Application.MarketDataService;
using Application.StockHoldingService;
using Domain.DTO.StockAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PortfolioCompositionService
{
    public class PortfolioCompositionService : IPortfolioCompositionService
    {
        private readonly IStockHoldingService _stockHoldingService;
        private readonly IMarketDataService _marketDataService;
        public PortfolioCompositionService(IStockHoldingService stockHoldingService, IMarketDataService marketDataService)
        {
            _stockHoldingService = stockHoldingService;
            _marketDataService = marketDataService;
        }

        public async Task<PortfolioComposition> GetPortfolioComposition(ulong userId)
        {
            var portfolioComposition = new PortfolioComposition();
            var individualWeightages = new List<IndividualStockWeightage>();

            var holdings = await _stockHoldingService.GetAllHoldingsForUserWithStockDetails(userId);
            var tickers = holdings.Select(x => x.Stock.Ticker);
            var lastQuoteForStocks = await _marketDataService.GetLastStockQuote(tickers);
            var tickerLastQuoteMap = lastQuoteForStocks.ToDictionary(x => x.Symbol);

            foreach (var holding in holdings)
            {
                var individualWeightage = new IndividualStockWeightage();
                individualWeightage.Ticker = holding.Stock.Ticker;

                foreach (var holdingDetail in holding.HoldingDetails)
                {
                    individualWeightage.TotalCurrentValue += tickerLastQuoteMap[individualWeightage.Ticker].Price * holdingDetail.Quantity;
                    individualWeightage.TotalPurchasePrice += holdingDetail.Price * holdingDetail.Quantity;
                }

                portfolioComposition.TotalCost += individualWeightage.TotalPurchasePrice;
                portfolioComposition.TotalInvestmentValue += individualWeightage.TotalCurrentValue;

                individualWeightages.Add(individualWeightage);
            }

            foreach (var individualWeightage in individualWeightages)
            {
                individualWeightage.PurchasePriceRatio = Math.Round(((individualWeightage.TotalPurchasePrice / portfolioComposition.TotalCost) * 100), 2);
                individualWeightage.CurrentValueRatio = Math.Round((individualWeightage.TotalCurrentValue / portfolioComposition.TotalInvestmentValue) * 100, 2);
            }

            portfolioComposition.IndividualStockWeightages = individualWeightages;

            return portfolioComposition;
        }
    }
}
