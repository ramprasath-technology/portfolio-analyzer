﻿using Application.StockHoldingService;
using Application.StockService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Domain;
using Application.StockQuoteService;
using Application.MarketDataService;
using Application.StockPurchaseService;
using Domain.DTO.StockAnalysis;
using Domain.DTO.ExternalData;

namespace Application.StockReturnsService
{
    public class StockReturnsService : IStockReturnsService
    {
        private readonly IStockHoldingService _stockHoldingService;
        private readonly IStockService _stockService;
        private readonly IStockPurchaseService _stockPurchaseService; 
        private readonly IMarketDataService _marketDataService;
        public StockReturnsService(IStockHoldingService stockHoldingService, 
            IStockService stockService,
            IStockPurchaseService stockPurchaseService,
            IMarketDataService marketDataService)
        {
            _stockHoldingService = stockHoldingService;
            _stockService = stockService;
            _stockPurchaseService = stockPurchaseService;
            _marketDataService = marketDataService;
        }

        public async Task<IEnumerable<StockAnnualizedReturn>> GetAnnualizedReturnForCurrentHoldings(ulong userId, uint? monthsSincePurchase)
        {
            var toDate = monthsSincePurchase == null ? DateTime.MaxValue : DateTime.Today.AddMonths((int)-monthsSincePurchase);
            var currentHoldings = await _stockHoldingService.GetAllHoldingsForUser(userId);
            var purchaseIds = ExtractPurchaseIds(currentHoldings);
            var purchaseTask = _stockPurchaseService.GetPurchasesByIdFilteredByDates(userId, purchaseIds, DateTime.MinValue, toDate);
            var purchases = await purchaseTask;
            var stockIds = FilterStockIds(purchases, currentHoldings);
            var stockTask = _stockService.GetStocksById(userId, stockIds);
            var stocks = await stockTask;
           
            var tickers = stocks.Select(x => x.Ticker);
            var stockQuotesTask = _marketDataService.GetLastStockQuote(tickers);
            var stockQuotes = await stockQuotesTask;
            

            var stockReturnModel = BuildStockReturnModel(stocks.ToDictionary(x => x.StockId), 
                purchases.ToDictionary(x => x.PurchaseId), 
                stockQuotes.ToDictionary(x => x.Symbol), 
                currentHoldings);

            ComputeAnnualizedReturn(stockReturnModel);

            stockReturnModel = stockReturnModel.OrderByDescending(x => x.AnnualizedReturn);

            return stockReturnModel;
        }

        private IEnumerable<ulong> FilterStockIds(IEnumerable<Purchase> purchases, IEnumerable<Holdings> holdings)
        {
            var stockIds = new List<ulong>();
            var stockIdsToConsider = new HashSet<ulong>();

            foreach (var purchase in purchases)
            {
                stockIdsToConsider.Add(purchase.StockId);
            }

            foreach (var holding in holdings)
            {
                if (stockIdsToConsider.Contains(holding.StockId))
                {
                    stockIds.Add(holding.StockId);
                }
            }

            return stockIds;
        }

        private IEnumerable<ulong> ExtractPurchaseIds(IEnumerable<Holdings> holding)
        {
            var purchaseIds = new HashSet<ulong>();

            foreach(var userHolding in holding)
            {
                var holdingDetails = userHolding.HoldingDetails;
                foreach(var detail in holdingDetails)
                {
                    purchaseIds.Add(detail.PurchaseId);
                }
            }

            return purchaseIds;
        }

        private IEnumerable<StockAnnualizedReturn> BuildStockReturnModel(Dictionary<ulong, Stock> stock, 
            Dictionary<ulong, Purchase> purchase, 
            Dictionary<string, LastStockQuote> lastQuote,
            IEnumerable<Holdings> holding)
        {
            var stockReturnInputs = new List<StockAnnualizedReturn>();

            foreach (var userHolding in holding)
            {
                var holdingDetails = userHolding.HoldingDetails;
                foreach (var detail in holdingDetails)
                {
                    if (purchase.ContainsKey(detail.PurchaseId)) //excludes holdings not in the filter date range
                    {
                        var purchaseData = purchase[detail.PurchaseId];
                        var stockData = stock[purchaseData.StockId];
                        if (lastQuote.ContainsKey(stockData.Ticker))
                        {
                            var annualizedReturnInput = new StockAnnualizedReturn();
                            annualizedReturnInput.CurrentDate = DateTime.Now.Date;
                            annualizedReturnInput.CurrentPrice = lastQuote[stockData.Ticker].Price;
                            annualizedReturnInput.PurchaseDate = purchaseData.Date;
                            annualizedReturnInput.PurchasePrice = purchaseData.Price;
                            annualizedReturnInput.Quantity = purchaseData.Quantity;
                            annualizedReturnInput.Ticker = stockData.Ticker;

                            stockReturnInputs.Add(annualizedReturnInput);
                        }
                    }
                }
            }

            return stockReturnInputs;
        }

        private void ComputeAnnualizedReturn(IEnumerable<StockAnnualizedReturn> annualizedStockReturn)
        {
            const short numberOfDaysInYear = 365;

            foreach(var stockReturn in annualizedStockReturn)
            {
                var numberOfDaysStockHeld = (stockReturn.CurrentDate.Date - stockReturn.PurchaseDate.Date).TotalDays;
                numberOfDaysStockHeld = numberOfDaysStockHeld == 0 ? 1 : numberOfDaysStockHeld;
                var daysRatio = Math.Round(numberOfDaysInYear / numberOfDaysStockHeld, 2);
                var cumulativeReturn = Math.Round(((stockReturn.CurrentPrice - stockReturn.PurchasePrice) / stockReturn.PurchasePrice), 2);
                stockReturn.TotalReturn = cumulativeReturn;
                stockReturn.AnnualizedReturn = Math.Round(Math.Pow(1 + Decimal.ToDouble(cumulativeReturn), daysRatio) - 1, 2);
                stockReturn.AnnualizedReturnDescription = (stockReturn.AnnualizedReturn * 100) + "%";
                stockReturn.TotalReturnDescription = (stockReturn.TotalReturn * 100) + "%";
            }
        }

    }
}
