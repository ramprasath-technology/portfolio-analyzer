using Application.Connection;
using Application.StockHoldingService;
using Application.StockPurchaseService;
using Application.StockService;
using Application.UserService;
using Domain;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.StockSplitService
{
    public class StockSplitService : IStockSplitService
    {
        private readonly IStockPurchaseService _stockPurchaseService;
        private readonly IStockHoldingService _holdingService;

        public StockSplitService(IStockPurchaseService stockPurchaseService,
            IStockHoldingService holdingService)
        {
            _stockPurchaseService = stockPurchaseService;
            _holdingService = holdingService;
        }

        public async Task PerformStockSplit(ulong userId, StockSplit splitDetails)
        {
            
            var purchaseTask = _stockPurchaseService.GetAllPurchasesForTicker(userId, splitDetails.Ticker);
            var holdingTask = _holdingService.GetAllHoldingsForUserWithStockDetails(userId);
            var purchases = await purchaseTask;
            var holdings = await holdingTask;
            var requiredHolding = holdings.Where(x => x.Stock.Ticker == splitDetails.Ticker).First();
            UpdatePurchases(purchases, splitDetails);
            UpdateUserHoldings(requiredHolding, splitDetails);
            await _stockPurchaseService.UpdatePurchasePriceAndQuantityByPurchaseId(userId, purchases);
            await _holdingService.UpdateHoldingDetails(userId, requiredHolding);      
        }

        private void UpdateUserHoldings(Holdings holding, StockSplit splitDetails)
        {
            var ratio = splitDetails.NewStockRatio / splitDetails.OldStockRatio;          
            if (holding != null)
            {
                var holdingDetails = holding.HoldingDetails;
                foreach (var detail in holdingDetails)
                {
                    detail.Price = detail.Price / ratio;
                    detail.Quantity = detail.Quantity * ratio;
                }
            }
            
        }

        private void UpdatePurchases(IEnumerable<Purchase> purchases, StockSplit splitDetails)
        {
            var applicablePurchases = purchases.Where(x => x.Date < splitDetails.EffectiveDate);
            if (splitDetails.LastSplitDate != null)
            {
                applicablePurchases = applicablePurchases.Where(x => x.Date >= splitDetails.LastSplitDate);
            }
            if (splitDetails.NewStockRatio > splitDetails.OldStockRatio)
            {
                var ratio = splitDetails.NewStockRatio / splitDetails.OldStockRatio;
                foreach (var purchase in purchases)
                {
                    purchase.Price = purchase.Price / ratio;
                    purchase.Quantity = purchase.Quantity * ratio;
                }
            }
        }
    }
}
