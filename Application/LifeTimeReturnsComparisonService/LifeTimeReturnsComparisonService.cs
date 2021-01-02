using Application.Connection;
using Application.MarketDataService;
using Application.StockPurchaseService;
using Application.StockSaleService;
using Domain;
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

        public LifeTimeReturnsComparisonService(IStockPurchaseService stockPurchaseService,
            IStockSaleService stockSaleService,
            IConnectionService connectionService,
            IMarketDataService marketDataService)
        {
            _stockPurchaseService = stockPurchaseService;
            _stockSaleService = stockSaleService;
            _connectionService = connectionService;
            _marketDataService = marketDataService;
        }

        public async Task<LifeTimeComparison> GetLifeTimeComparison(ulong userId, 
            IDbConnection connection = null)
        {
            if (connection == null)
            {
                connection = _connectionService.GetConnection(userId);
            }
            
            var lifeTimeComparison = new LifeTimeComparison();

            using (connection)
            {
                var purchases = await _stockPurchaseService.GetPurchasesForUserWithStockData(userId, connection);
                var purchaseIdPurchaseMap = purchases.ToDictionary(x => x.PurchaseId);
                var sales = await _stockSaleService.GetSalesByPurchaseIds(userId, purchases.Select(x => x.PurchaseId));
                var purchaseIsSaleMap = GetPurchaseIdSaleMap(sales);

                foreach (var sale in sales)
                {
                    if (purchaseIdPurchaseMap.ContainsKey(sale.PurchaseId))
                    {
                        var correspondingPurchase = purchaseIdPurchaseMap[sale.PurchaseId];
                        var totalSale = sale.Price * sale.Quantity;
                        lifeTimeComparison.TotalRealizedGainOrLoss += (totalSale - (correspondingPurchase.Price * sale.Quantity));
                        correspondingPurchase.Quantity -= sale.Quantity;
                        lifeTimeComparison.TotalSale += (sale.Quantity * sale.Price);
                        lifeTimeComparison.TotalPurchase += (correspondingPurchase.Price * sale.Quantity);
                    }                  
                }

                var tickers = GetDistinctTickers(purchaseIdPurchaseMap.Values);
                var lastPrices = await _marketDataService.GetLastStockQuote(tickers);
                var tickerPriceMap = lastPrices.ToDictionary(x => x.Symbol);

                foreach (var purchase in purchaseIdPurchaseMap.Values)
                {
                    if (purchase.Quantity > 0 && tickerPriceMap.ContainsKey(purchase.Stock.Ticker))
                    {
                        lifeTimeComparison.TotalPaperGainOrLoss += ((tickerPriceMap[purchase.Stock.Ticker].Price * purchase.Quantity) - (purchase.Price * purchase.Quantity));
                        lifeTimeComparison.TotalPurchase += (purchase.Price * purchase.Quantity);
                    }
                }
            }

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
