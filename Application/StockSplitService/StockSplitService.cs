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
            var purchaseIdsUpdated = UpdatePurchases(purchases, splitDetails);
            UpdateUserHoldings(requiredHolding, splitDetails, purchaseIdsUpdated);
            await _stockPurchaseService.UpdatePurchasePriceAndQuantityByPurchaseId(userId, purchases);
            await _holdingService.UpdateHoldingDetails(userId, requiredHolding);      
        }

        private void UpdateUserHoldings(Holdings holding, 
            StockSplit splitDetails, 
            IEnumerable<ulong> purchaseIdsUpdated)
        {
            var ratio = splitDetails.NewStockRatio / splitDetails.OldStockRatio;          
            if (holding != null && purchaseIdsUpdated.Count() > 0)
            {
                var holdingDetails = holding.HoldingDetails;
                foreach (var detail in holdingDetails)
                {
                    if (purchaseIdsUpdated.Contains(detail.PurchaseId))
                    {
                        detail.Price = detail.Price / ratio;
                        detail.Quantity = detail.Quantity * ratio;
                    }
                }
            }
            
        }

        private IEnumerable<ulong> UpdatePurchases(IEnumerable<Purchase> purchases, StockSplit splitDetails)
        {
            var applicablePurchases = purchases.Where(x => x.Date < splitDetails.EffectiveDate);
            var purchaseIdsUpdated = new List<ulong>();
            if (splitDetails.LastSplitDate != null)
            {
                applicablePurchases = applicablePurchases.Where(x => x.Date >= splitDetails.LastSplitDate);
            }
            if (splitDetails.NewStockRatio > splitDetails.OldStockRatio)
            {
                var ratio = splitDetails.NewStockRatio / splitDetails.OldStockRatio;
                foreach (var purchase in applicablePurchases)
                {
                    purchase.Price = purchase.Price / ratio;
                    purchase.Quantity = purchase.Quantity * ratio;
                    purchaseIdsUpdated.Add(purchase.PurchaseId);
                }
            }
            return purchaseIdsUpdated;
        }
    }
}
