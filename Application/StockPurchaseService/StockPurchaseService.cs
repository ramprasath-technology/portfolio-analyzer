﻿using Application.Connection;
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

        public async Task<Purchase> AddStockPurchase(ulong userId, Purchase purchase)
        {
            var connection = _connectionService.GetConnection(userId);

            await _stockPurchaseData.AddPurchase(purchase, connection);

            _connectionService.DisposeConnection(connection);

            return purchase;
        }

        public async Task<IEnumerable<Purchase>> GetPurchasesById(ulong userId, IEnumerable<ulong> purchaseId)
        {
            var connection = _connectionService.GetConnection(userId);

            var purchases = await _stockPurchaseData.GetPurchasesById(connection, purchaseId);

            _connectionService.DisposeConnection(connection);

            return purchases;
        }
    }
}
