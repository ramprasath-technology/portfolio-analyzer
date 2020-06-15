using Application.MarketDataService;
using Application.StockHoldingService;
using Application.StockPurchaseService;
using Domain;
using Domain.DTO.StockAnalysis;
using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockTotalValueService
{
    public class StockTotalValueService : IStockTotalValueService
    {
        private readonly IStockHoldingService _stockHoldingService;
        private readonly IMarketDataService _marketDataService;
        private readonly IStockPurchaseService _stockPurchaseService;
        public StockTotalValueService(IStockHoldingService stockHoldingService, 
            IMarketDataService marketDataService,
            IStockPurchaseService stockPurchaseService)
        {
            _stockHoldingService = stockHoldingService;
            _marketDataService = marketDataService;
            _stockPurchaseService = stockPurchaseService;
        }

        public async Task<TotalHoldingsValue> GetTotalValueForUserHolding(ulong userId)
        {
            var holdings = await _stockHoldingService.GetAllHoldingsForUserWithStockDetails(userId);
            /*
            var tickers = GetStockTickersFromHoldings(holdings);
            var stockPricesTask = _marketDataService.GetLastStockQuote(tickers);
            var lastStockQuote = await stockPricesTask;
            var tickerStockQuoteMap = MapTickerToLastStockQuote(lastStockQuote);
            var individualStockValues = new List<IndividualStockValue>();

            foreach(var holding in holdings)
            {
                var ticker = holding.Stock.Ticker;
                var holdingDetails = holding.HoldingDetails;
                var individualStockValue = new IndividualStockValue();
                individualStockValue.Ticker = ticker;

                foreach (var holdingDetail in holdingDetails)
                {
                    individualStockValue.TotalStockCurrentPrice += tickerStockQuoteMap[ticker] * holdingDetail.Quantity;
                    individualStockValue.TotalStockPurchasePrice += holdingDetail.Price * holdingDetail.Quantity;
                }

                individualStockValue.TotalStockGainOrLoss = individualStockValue.TotalStockCurrentPrice - individualStockValue.TotalStockPurchasePrice;
                individualStockValue.TotalStockGainOrLossPercentage = (individualStockValue.TotalStockGainOrLoss / individualStockValue.TotalStockPurchasePrice) * 100;
                individualStockValues.Add(individualStockValue);
            }

            return ComputeTotalHoldingsValue(individualStockValues);*/
            return new TotalHoldingsValue();
        }

        private TotalHoldingsValue ComputeTotalHoldingsValue(IEnumerable<IndividualStockValue> individualStockValues)
        {
            var totalHoldingValue = new TotalHoldingsValue();

            totalHoldingValue.TotalHoldingPurchasePrice = individualStockValues.Sum(x => x.TotalStockPurchasePrice);
            totalHoldingValue.TotalHoldingCurrentPrice = individualStockValues.Sum(x => x.TotalStockCurrentPrice);
            totalHoldingValue.TotalHoldingGainOrLoss = totalHoldingValue.TotalHoldingCurrentPrice - totalHoldingValue.TotalHoldingPurchasePrice;
            totalHoldingValue.TotalHoldingGainOrLossPercentage = (totalHoldingValue.TotalHoldingGainOrLoss / totalHoldingValue.TotalHoldingPurchasePrice) * 100;
            totalHoldingValue.IndividualStockValues = individualStockValues;

            return totalHoldingValue;
        }

        private IEnumerable<string> GetStockTickersFromHoldings(IEnumerable<Holdings> holdings)
        {
            var tickers = new HashSet<string>();

            foreach (var holding in holdings)
            {
                tickers.Add(holding.Stock.Ticker);
            }

            return tickers;
        }

        private Dictionary<string, decimal> MapTickerToLastStockQuote(IEnumerable<LastStockQuote> stockQuotes)
        {
            var ticketLastStockQuoteMap = new Dictionary<string, decimal>();

            foreach (var stockQuote in stockQuotes)
            {
                ticketLastStockQuoteMap[stockQuote.Symbol] = stockQuote.Price;
            }

            return ticketLastStockQuoteMap;
        }
    }
}
