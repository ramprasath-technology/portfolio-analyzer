using Application.Connection;
using Application.MarketDataService;
using Application.StockIndexValueService;
using Application.StockPurchaseService;
using Application.StockSaleService;
using Domain;
using Domain.DTO;
using Domain.DTO.StockAnalysis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.LifeTimeReturnsComparisonService
{
    public class LifeTimeReturnsComparisonService : ILifeTimeReturnsComparisonService
    {
        private readonly IStockPurchaseService _stockPurchaseService;
        private readonly IStockSaleService _stockSaleService;
        private readonly IConnectionService _connectionService;
        private readonly IMarketDataService _marketDataService;
        private readonly IStockIndexValueService _stockIndexValueService;

        public LifeTimeReturnsComparisonService(IStockPurchaseService stockPurchaseService,
            IStockSaleService stockSaleService,
            IConnectionService connectionService,
            IMarketDataService marketDataService,
            IStockIndexValueService stockIndexValueService)
        {
            _stockPurchaseService = stockPurchaseService;
            _stockSaleService = stockSaleService;
            _connectionService = connectionService;
            _marketDataService = marketDataService;
            _stockIndexValueService = stockIndexValueService;
        }

        public async Task<LifeTimeComparison> GetLifeTimeComparison(ulong userId, 
            IDbConnection connection = null)
        {
            if (connection == null)
            {
                connection = _connectionService.GetConnection(userId);
            }
            
            var lifeTimeComparison = new LifeTimeComparison();
            var indexLifeTimeComparisonMap = new Dictionary<string, IndexLifeTimeComparison>();

            using (connection)
            {
                var purchases = await _stockPurchaseService.GetPurchasesForUserWithStockData(userId, connection);               
                var purchaseIdPurchaseMap = purchases.ToDictionary(x => x.PurchaseId);
                var sales = await _stockSaleService.GetSalesByPurchaseIds(userId, purchases.Select(x => x.PurchaseId));
                var purchaseAndSaleDates = new List<DateTime>();
                purchaseAndSaleDates.AddRange(purchases.Select(x => x.Date));
                purchaseAndSaleDates.AddRange(sales.Select(x => x.Date));
                var tickerPricesOnPurchaseDate = await _stockIndexValueService.GetPricesForGivenIndexTickersAndDates(userId, IndexTickers.GetAllowedIndexTickers(), purchaseAndSaleDates);
                var purchasesByIndexTickerAndDates = _stockIndexValueService.OrderIndexValuesByDateAndTicker(tickerPricesOnPurchaseDate);
                var purchaseIsSaleMap = GetPurchaseIdSaleMap(sales);

                foreach (var sale in sales)
                {
                    if (purchaseIdPurchaseMap.ContainsKey(sale.PurchaseId))
                    {
                        var correspondingPurchase = purchaseIdPurchaseMap[sale.PurchaseId];
                        var totalSale = sale.Price * sale.Quantity;
                        lifeTimeComparison.TotalRealizedGainOrLoss += (totalSale - (correspondingPurchase.Price * sale.Quantity));                     
                        lifeTimeComparison.TotalSale += (sale.Quantity * sale.Price);
                        lifeTimeComparison.TotalPurchase += (correspondingPurchase.Price * sale.Quantity);
                        var saleProportion = sale.Quantity / correspondingPurchase.Quantity;
                        foreach (var indexTicker in IndexTickers.GetAllowedIndexTickers())
                        {
                            var priceOnPurchaseDate = purchasesByIndexTickerAndDates[correspondingPurchase.Date.Date][indexTicker];
                            var purchaseQuantity = (correspondingPurchase.Quantity * correspondingPurchase.Price) / priceOnPurchaseDate;
                            var priceOnSaleDate = purchasesByIndexTickerAndDates[sale.Date.Date][indexTicker];
                            var saleQuantity = saleProportion * purchaseQuantity;
                            var realizedGainOrLoss = (priceOnSaleDate - priceOnPurchaseDate) * (saleQuantity);
                            
                            if (!indexLifeTimeComparisonMap.ContainsKey(indexTicker))
                            {
                                var lifeTimeComparisonForIndex = new IndexLifeTimeComparison();
                                indexLifeTimeComparisonMap.Add(indexTicker, lifeTimeComparisonForIndex);
                            }
                            indexLifeTimeComparisonMap[indexTicker].TotalRealizedGainOrLoss += realizedGainOrLoss;
                            indexLifeTimeComparisonMap[indexTicker].TotalPurchaseUnits += purchaseQuantity;
                            indexLifeTimeComparisonMap[indexTicker].TotalSaleUnits += saleQuantity;
                        }
                        correspondingPurchase.Quantity -= sale.Quantity;
                    }                  
                }

                var tickers = GetDistinctTickers(purchaseIdPurchaseMap.Values).ToList();
                tickers.AddRange(IndexTickers.GetAllowedIndexTickers());
                var lastPrices = await _marketDataService.GetLastStockQuote(tickers);
                var tickerPriceMap = lastPrices.ToDictionary(x => x.Symbol);

                foreach (var purchase in purchaseIdPurchaseMap.Values)
                {
                    if (purchase.Quantity > 0 && tickerPriceMap.ContainsKey(purchase.Stock.Ticker))
                    {
                        lifeTimeComparison.TotalPaperGainOrLoss += ((tickerPriceMap[purchase.Stock.Ticker].Price * purchase.Quantity) - (purchase.Price * purchase.Quantity));
                        lifeTimeComparison.TotalPurchase += (purchase.Price * purchase.Quantity);
                        foreach (var indexTicker in IndexTickers.GetAllowedIndexTickers())
                        {
                            var indexPurchaseUnits = (purchase.Price * purchase.Quantity) / purchasesByIndexTickerAndDates[purchase.Date.Date][indexTicker];
                            if (!indexLifeTimeComparisonMap.ContainsKey(indexTicker))
                            {
                                var indexLifeTimeComparison = new IndexLifeTimeComparison();
                                indexLifeTimeComparisonMap.Add(indexTicker, indexLifeTimeComparison);
                            }
                            indexLifeTimeComparisonMap[indexTicker].TotalPurchaseUnits += indexPurchaseUnits;
                            indexLifeTimeComparisonMap[indexTicker].TotalPaperGainOrLoss += ((tickerPriceMap[indexTicker].Price * indexPurchaseUnits) - (purchase.Price * purchase.Quantity));
                        }
                    }
                }
            }

            lifeTimeComparison.IndexLifeTimeComparisonMap = indexLifeTimeComparisonMap;
            foreach (var indexComparison in lifeTimeComparison.IndexLifeTimeComparisonMap.Values)
            {
                indexComparison.TotalGainOrLoss = indexComparison.TotalPaperGainOrLoss + indexComparison.TotalRealizedGainOrLoss;
                indexComparison.TotalReturnPercentage = (indexComparison.TotalGainOrLoss / lifeTimeComparison.TotalPurchase) * 100;
            }

            lifeTimeComparison.TotalGainOrLoss = lifeTimeComparison.TotalPaperGainOrLoss + lifeTimeComparison.TotalRealizedGainOrLoss;
            lifeTimeComparison.TotalReturnPercentage = (lifeTimeComparison.TotalGainOrLoss / lifeTimeComparison.TotalPurchase) * 100;

            return lifeTimeComparison;
        }

        private IEnumerable<string> GetDistinctTickers(IEnumerable<Purchase> purchases)
        {
            var tickers = new HashSet<string>();

            foreach(var purchase in purchases)
            {
                if (purchase.Quantity > 0)
                {
                    tickers.Add(purchase.Stock.Ticker);
                }
            }

            return tickers;
        }

        private Dictionary<ulong, List<Sale>> GetPurchaseIdSaleMap(IEnumerable<Sale> sales)
        {
            var purchaseIdSaleMap = new Dictionary<ulong, List<Sale>>();

            foreach (var sale in sales)
            {
                if (purchaseIdSaleMap.ContainsKey(sale.PurchaseId))
                {
                    purchaseIdSaleMap[sale.PurchaseId].Add(sale);
                }
                else
                {
                    var salesForThisPurchase = new List<Sale>();
                    salesForThisPurchase.Add(sale);
                    purchaseIdSaleMap[sale.PurchaseId] = salesForThisPurchase;
                }
            }

            return purchaseIdSaleMap;
        }
    }
}
