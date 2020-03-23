using Application.Connection;
using Domain;
using Persistence.StockPurchaseData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockPurchaseService
{
    public class StockPurchaseService : IStockPurchaseService
    {
        private readonly IStockPurchaseData _stockPurchaseData;
        private readonly IConnectionService _connectionService;
        public StockPurchaseService(IStockPurchaseData stockPurchaseData, IConnectionService connectionService)
        {
            _stockPurchaseData = stockPurchaseData;
            _connectionService = connectionService;
        }

        public async Task<ulong> AddStockPurchase(ulong userId, Purchase purchase)
        {
            var purchaseId = await _stockPurchaseData.AddPurchase(purchase, _connectionService.GetConnection(userId));
            return purchaseId;
        }
    }
}
