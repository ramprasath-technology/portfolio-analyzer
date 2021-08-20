using Application.MarketDataService;
using Application.StockHoldingService;
using Application.StockIndexValueService;
using Application.StockPurchaseService;
using Application.StockService;
using Domain;
using Domain.DTO.StockAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockIndexComparisonService
{
    public class StockIndexComparisonService : IStockIndexComparisonService
    {
        private readonly IStockHoldingService _stockHoldingService;
        private readonly IStockIndexValueService _stockIndexValueService;
        private readonly IStockService _stockService;
        private readonly IStockPurchaseService _stockPurchaseService;
        private readonly IMarketDataService _marketDataService;

        public StockIndexComparisonService(IStockHoldingService stockHoldingService, 
            IStockIndexValueService stockIndexValueService,
            IStockService stockService,
            IStockPurchaseService stockPurchaseService,
            IMarketDataService marketDataService)
        {
            _stockHoldingService = stockHoldingService;
            _stockIndexValueService = stockIndexValueService;
            _stockService = stockService;
            _stockPurchaseService = stockPurchaseService;
            _marketDataService = marketDataService;
        }

        public async Task<IEnumerable<StockComparisonToIndex>> GetComparisonWithIndex(ulong userId, 
            IEnumerable<string> indexTickers,
            DateTime from = default(DateTime),
            DateTime to = default(DateTime))
        {
            var stockIndexComparisons = new List<StockComparisonToIndex>();
            var holdings = await _stockHoldingService.GetAllHoldingsForUser(userId);
            var stockIds = GetStockIdsFromHolding(holdings);
            var stockTask = _stockService.GetStocksById(userId, stockIds);
            var purchaseIds = GetPurchaseIdsFromHolding(holdings);
            var stocks = await stockTask;
            var purchaseTask = GetPurchases(userId, purchaseIds, from, to);
            var purchases = await purchaseTask;
            var stockIdsInPurchases = purchases.Select(x => x.StockId).Distinct();
            var tickers = GetTickerFromStockInformation(stocks.Where(x => stockIdsInPurchases.Contains(x.StockId))).ToList();           
            tickers.AddRange(indexTickers);
            var stockQuoteTask = _marketDataService.GetLastStockQuote(tickers);
            var stockIdTickerMap = MapStockIdToTicker(stocks);
            var purchaseDates = GetPurchaseDates(purchases);
            var indexValues = await _stockIndexValueService.GetPricesForGivenIndexTickersAndDates(userId, indexTickers, purchaseDates);
            var dateIndexValueMap = MapIndexValuesToDate(indexValues);
            var stockQuotes = await stockQuoteTask;
            var tickerStockQuoteMap = stockQuotes.ToDictionary(x => x.Symbol);
            

            foreach(var purchase in purchases)
            {
                var ticker = stockIdTickerMap[purchase.StockId];
                if (!tickerStockQuoteMap.ContainsKey(ticker))
                    continue;
                var stockPriceToday = tickerStockQuoteMap[ticker].Price;
                var stockIndexComparison = new StockComparisonToIndex();
                stockIndexComparison.Ticker = ticker;
                stockIndexComparison.CurrentPrice = stockPriceToday;
                stockIndexComparison.PurchaseDate = purchase.Date;
                stockIndexComparison.PurchasePrice = purchase.Price;
                stockIndexComparison.Quantity = purchase.Quantity;
                stockIndexComparison.TotalPriceDifference = Decimal.Round((stockPriceToday - purchase.Price) * purchase.Quantity, 4);
                stockIndexComparison.PriceDifferencePerQuantity = stockPriceToday - purchase.Price;
                stockIndexComparison.TotalPurchasePrice = purchase.Price * purchase.Quantity;
                stockIndexComparison.TotalCurrentPrice = stockPriceToday * purchase.Quantity;
                stockIndexComparison.PercentageChange = Decimal.Round((stockIndexComparison.TotalPriceDifference / stockIndexComparison.TotalPurchasePrice) * 100, 4);
                stockIndexComparison.IndexesDifference = new List<IndexDifference>();

                foreach (var indexTicker in indexTickers)
                {
                    var indexPriceToday = tickerStockQuoteMap[indexTicker].Price;
                    var indexPriceOnPurchaseDate = dateIndexValueMap[purchase.Date.Date][indexTicker]; 
                    var stockPriceToIndexPriceRatio = stockIndexComparison.PurchasePrice / indexPriceOnPurchaseDate; 
                    var indexDifference = new IndexDifference();
                    indexDifference.CurrentPrice = indexPriceToday;
                    indexDifference.IndexTicker = indexTicker;
                    indexDifference.PurchaseQuantity = stockPriceToIndexPriceRatio * purchase.Quantity;
                    indexDifference.PriceDifferencePerQuantity = Decimal.Round((indexPriceToday - indexPriceOnPurchaseDate) * stockPriceToIndexPriceRatio, 4);               
                    indexDifference.TotalPriceDifference = Decimal.Round((indexPriceToday - indexPriceOnPurchaseDate) * indexDifference.PurchaseQuantity, 4);
                    indexDifference.PriceOnPurchaseDate = indexPriceOnPurchaseDate;
                    indexDifference.TotalPurchasePrice = indexDifference.PriceOnPurchaseDate * indexDifference.PurchaseQuantity;
                    indexDifference.PercentageChange = Decimal.Round((indexDifference.TotalPriceDifference / indexDifference.TotalPurchasePrice) * 100, 4);
                    indexDifference.TotalCurrentPrice = indexDifference.CurrentPrice * indexDifference.PurchaseQuantity;
                    stockIndexComparison.IndexesDifference.Add(indexDifference);
                }

                stockIndexComparisons.Add(stockIndexComparison);
            }

            return stockIndexComparisons;
        }

        private Task<IEnumerable<Purchase>> GetPurchases(ulong userId, 
            IEnumerable<ulong> purchaseIds, 
            DateTime from, 
            DateTime to)
        {
            if (from != DateTime.MinValue && to != DateTime.MinValue && from <= to)
                return _stockPurchaseService.GetPurchasesByIdFilteredByDates(userId, purchaseIds, from, to);

            else
                return _stockPurchaseService.GetPurchasesById(userId, purchaseIds);
        }

        private IEnumerable<string> GetTickersToCompare(IEnumerable<Purchase> purchases, IEnumerable<string> allTickers)
        {
            var tickersFromPurchase = new HashSet<string>();
            var tickers = new HashSet<string>();

            foreach (var purchase in purchases)
            {
                tickersFromPurchase.Add(purchase.Stock.Ticker);
            }

            foreach (var ticker in allTickers)
            {
                if (tickersFromPurchase.Contains(ticker))
                {
                    tickers.Add(ticker);
                }
            }

            return tickers;
        }

        private IEnumerable<ulong> GetStockIdsFromHolding(IEnumerable<Holdings> holding)
        {
            var stockIds = new HashSet<ulong>();

            foreach(var stockHolding in holding)
            {
                stockIds.Add(stockHolding.StockId);
            }

            return stockIds;
        }

        private IEnumerable<ulong> GetPurchaseIdsFromHolding(IEnumerable<Holdings> holding)
        {
            var purchaseIds = new HashSet<ulong>();

            foreach(var userHolding in holding)
            {
                var holdingsDetail = userHolding.HoldingDetails;

                foreach(var holdingDetail in holdingsDetail)
                {
                    purchaseIds.Add(holdingDetail.PurchaseId);
                }
            }

            return purchaseIds;
        }

        private IEnumerable<string> GetTickerFromStockInformation(IEnumerable<Stock> stocks)
        {
            var tickers = new HashSet<string>();

            foreach(var stock in stocks)
            {
                tickers.Add(stock.Ticker);
            }

            return tickers;
        }

        private Dictionary<ulong, string> MapStockIdToTicker(IEnumerable<Stock> stocks)
        {
            var stockIdTickerMap = new Dictionary<ulong, string>();

            foreach(var stock in stocks)
            {
                if(!stockIdTickerMap.ContainsKey(stock.StockId))
                {
                    stockIdTickerMap[stock.StockId] = stock.Ticker;
                }               
            }

            return stockIdTickerMap;
        }

        private IEnumerable<DateTime> GetPurchaseDates(IEnumerable<Purchase> purchases)
        {
            var purchaseDates = new HashSet<DateTime>();

            foreach(var purchase in purchases)
            {
                purchaseDates.Add(purchase.Date);
            }

            return purchaseDates;
        }

        private Dictionary<DateTime, Dictionary<string, decimal>> MapIndexValuesToDate(IEnumerable<StockIndexValue> stockIndexValues)
        {
            var dateIndexValueMap = new Dictionary<DateTime, Dictionary<string, decimal>>();

            foreach (var stockIndexValue in stockIndexValues)
            {               
                if (!dateIndexValueMap.ContainsKey(stockIndexValue.Date))
                {
                    var tickerPriceMap = new Dictionary<string, decimal>();
                    tickerPriceMap[stockIndexValue.StockIndexTicker.Ticker] = stockIndexValue.Value;
                    dateIndexValueMap[stockIndexValue.Date.Date] = tickerPriceMap;
                }
                else
                {
                    var existingTickerValueMap = dateIndexValueMap[stockIndexValue.Date.Date];
                    if (!existingTickerValueMap.ContainsKey(stockIndexValue.StockIndexTicker.Ticker))
                    {
                        existingTickerValueMap[stockIndexValue.StockIndexTicker.Ticker] = stockIndexValue.Value;
                    }
                }
            }

            return dateIndexValueMap;
        }
    }
}
