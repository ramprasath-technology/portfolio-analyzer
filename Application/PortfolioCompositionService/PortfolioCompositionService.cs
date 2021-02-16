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

            Parallel.ForEach(holdings, (holding) =>
            {
                var individualWeightage = new IndividualStockWeightage();
                individualWeightage.Ticker = holding.Stock.Ticker;
                individualWeightage.Country = holding.Stock.Country;
                individualWeightage.Sector = holding.Stock.Sector;
                individualWeightage.Industry = holding.Stock.Industry;

                foreach (var holdingDetail in holding.HoldingDetails)
                {
                    individualWeightage.TotalCurrentValue += tickerLastQuoteMap[individualWeightage.Ticker].Price * holdingDetail.Quantity;
                    individualWeightage.TotalPurchasePrice += holdingDetail.Price * holdingDetail.Quantity;
                }

                FindAmountsByCountry(individualWeightage, portfolioComposition);
                FindAmountsByIndustry(individualWeightage, portfolioComposition);
                FindAmountsBySector(individualWeightage, portfolioComposition);

                portfolioComposition.TotalCost += individualWeightage.TotalPurchasePrice;
                portfolioComposition.TotalInvestmentValue += individualWeightage.TotalCurrentValue;

                individualWeightages.Add(individualWeightage);
            });

            foreach (var individualWeightage in individualWeightages)
            {
                individualWeightage.PurchasePriceRatio = Math.Round(((individualWeightage.TotalPurchasePrice / portfolioComposition.TotalCost) * 100), 2);
                individualWeightage.CurrentValueRatio = Math.Round((individualWeightage.TotalCurrentValue / portfolioComposition.TotalInvestmentValue) * 100, 2);
            }

            Task.WaitAll(Task.Run(() => { FindRatiosByCountry(portfolioComposition); }),
                         Task.Run(() => { FindRatiosByIndustry(portfolioComposition); }),
                         Task.Run(() => { FindRatiosBySector(portfolioComposition); }));

            portfolioComposition.IndividualStockWeightages = individualWeightages;

            return portfolioComposition;
        }

        private void FindAmountsByCountry(IndividualStockWeightage individualWeightage, PortfolioComposition portfolioComposition)
        {
            if (!portfolioComposition.InvestmentAmountByCountry.ContainsKey(individualWeightage.Country))
            {
                portfolioComposition.InvestmentAmountByCountry[individualWeightage.Country] = individualWeightage.TotalPurchasePrice;
            }
            else
            {
                portfolioComposition.InvestmentAmountByCountry[individualWeightage.Country] += individualWeightage.TotalPurchasePrice;
            }

            if (!portfolioComposition.CurrentValueAmountByCountry.ContainsKey(individualWeightage.Country))
            {
                portfolioComposition.CurrentValueAmountByCountry[individualWeightage.Country] = individualWeightage.TotalCurrentValue;
            }
            else
            {
                portfolioComposition.CurrentValueAmountByCountry[individualWeightage.Country] += individualWeightage.TotalCurrentValue;
            }
        }

        private void FindAmountsByIndustry(IndividualStockWeightage individualWeightage, PortfolioComposition portfolioComposition)
        {
            if (!portfolioComposition.InvestmentAmountByIndustry.ContainsKey(individualWeightage.Industry))
            {
                portfolioComposition.InvestmentAmountByIndustry[individualWeightage.Industry] = individualWeightage.TotalPurchasePrice;
            }
            else
            {
                portfolioComposition.InvestmentAmountByIndustry[individualWeightage.Industry] += individualWeightage.TotalPurchasePrice;
            }

            if (!portfolioComposition.CurrentValueAmountByIndustry.ContainsKey(individualWeightage.Industry))
            {
                portfolioComposition.CurrentValueAmountByIndustry[individualWeightage.Industry] = individualWeightage.TotalCurrentValue;
            }
            else
            {
                portfolioComposition.CurrentValueAmountByIndustry[individualWeightage.Industry] += individualWeightage.TotalCurrentValue;
            }
        }

        private void FindAmountsBySector(IndividualStockWeightage individualWeightage, PortfolioComposition portfolioComposition)
        {
            if (!portfolioComposition.InvestmentAmountBySector.ContainsKey(individualWeightage.Sector))
            {
                portfolioComposition.InvestmentAmountBySector[individualWeightage.Sector] = individualWeightage.TotalPurchasePrice;
            }
            else
            {
                portfolioComposition.InvestmentAmountBySector[individualWeightage.Sector] += individualWeightage.TotalPurchasePrice;
            }

            if (!portfolioComposition.CurrentValueAmountBySector.ContainsKey(individualWeightage.Sector))
            {
                portfolioComposition.CurrentValueAmountBySector[individualWeightage.Sector] = individualWeightage.TotalCurrentValue;
            }
            else
            {
                portfolioComposition.CurrentValueAmountBySector[individualWeightage.Sector] += individualWeightage.TotalCurrentValue;
            }
        }

        private void FindRatiosByCountry(PortfolioComposition portfolioComposition)
        {
            foreach (var entry in portfolioComposition.InvestmentAmountByCountry)
            {
                portfolioComposition.InvestmentRatioByCountry[entry.Key] = entry.Value / portfolioComposition.TotalCost;
            }

            foreach (var entry in portfolioComposition.CurrentValueAmountByCountry)
            {
                portfolioComposition.CurrentValueRatioByCountry[entry.Key] = entry.Value / portfolioComposition.TotalInvestmentValue;
            }
        }

        private void FindRatiosBySector(PortfolioComposition portfolioComposition)
        {
            foreach (var entry in portfolioComposition.InvestmentAmountBySector)
            {
                portfolioComposition.InvestmentRatioBySector[entry.Key] = entry.Value / portfolioComposition.TotalCost;
            }

            foreach (var entry in portfolioComposition.CurrentValueAmountBySector)
            {
                portfolioComposition.CurrentValueRatioBySector[entry.Key] = entry.Value / portfolioComposition.TotalInvestmentValue;
            }
        }

        private void FindRatiosByIndustry(PortfolioComposition portfolioComposition)
        {
            foreach (var entry in portfolioComposition.InvestmentAmountByIndustry)
            {
                portfolioComposition.InvestmentRatioByIndustry[entry.Key] = entry.Value / portfolioComposition.TotalCost;
            }

            foreach (var entry in portfolioComposition.CurrentValueAmountByIndustry)
            {
                portfolioComposition.CurrentValueRatioByIndustry[entry.Key] = entry.Value / portfolioComposition.TotalInvestmentValue;
            }
        }
    }
}
