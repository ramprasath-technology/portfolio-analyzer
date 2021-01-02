using Application.Connection;
using Domain;
using Persistence.StockPurchaseData;
using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<IEnumerable<Purchase>> GetPurchasesByIdFilteredByDates(ulong userId, 
            IEnumerable<ulong> purchaseId,
            DateTime from,
            DateTime to)
        {
            using (var connection = _connectionService.GetOpenConnection(userId))
            {
                var purchases = await _stockPurchaseData.GetPurchasesByIdFilteredByDates(connection, purchaseId, from, to);

                return purchases;
            }
        }

        public async Task<IEnumerable<Purchase>> GetPurchasesForUser(ulong userId, 
            IDbConnection connection = null)
        {
            if (connection == null)
            {
                connection = _connectionService.GetOpenConnection(userId);
            }
            using (connection)
            {
                var purchases = await _stockPurchaseData.GetAllPurchasesForUser(connection, userId);
                return purchases;
            }
        }

        public async Task<IEnumerable<Purchase>> GetPurchasesForUserWithStockData(ulong userId,
            IDbConnection connection = null)
        {
            if (connection == null)
            {
                connection = _connectionService.GetOpenConnection(userId);
            }
            using (connection)
            {
                var purchases = await _stockPurchaseData.GetAllPurchasesForUserWithStockData(connection, userId);
                return purchases;
            }
        }

        public async Task<IEnumerable<Purchase>> GetAllPurchasesForTicker(ulong userId, string ticker)
        {
            using (var conn = _connectionService.GetOpenConnection(userId))
            {
                var purchases = await _stockPurchaseData.GetAllPurchasesByTicker(conn, userId, ticker);
                return purchases;
            }
        }

        public async Task UpdatePurchasePriceAndQuantityByPurchaseId(ulong userId, IEnumerable<Purchase> purchases)
        {
            using (var conn = _connectionService.GetOpenConnection(userId))
            {
                await _stockPurchaseData.UpdatePurchasePriceAndQuantityByPurchaseId(conn, purchases);
            }
        }
    }
}
