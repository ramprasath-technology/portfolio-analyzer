using Application.Connection;
using Application.StockHoldingService;
using Domain;
using Persistence.StockSaleData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.StockSaleService
{
    public class StockSaleService : IStockSaleService
    {
        private readonly IStockSaleData _stockSaleData;
        private readonly IConnectionService _connectionService;
        private readonly IStockHoldingService _stockHoldingService;

        public StockSaleService(IStockSaleData stockSaleData,
            IConnectionService connectionService,
            IStockHoldingService stockHoldingService)
        {
            _stockSaleData = stockSaleData;
            _connectionService = connectionService;
            _stockHoldingService = stockHoldingService;
        }

        public async Task<Sale> AddSale(ulong userId, Sale sale)
        {
            using (var scope = new TransactionScope())
            {
                var connection = _connectionService.GetConnection(userId);

                await _stockSaleData.AddSale(connection, sale);

                _connectionService.DisposeConnection(connection);

                await _stockHoldingService.UpdateHoldingAfterSale(userId, sale);

                scope.Complete();

                return sale;
            }
        }

    }
}
