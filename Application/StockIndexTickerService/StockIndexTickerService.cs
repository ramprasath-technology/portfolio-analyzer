using Application.Connection;
using Domain;
using Persistence.StockIndexTickerData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockIndexTickerService
{
    public class StockIndexTickerService : IStockIndexTickerService
    {
        private readonly IStockIndexTickerData _stockIndexTickerData;
        private readonly IConnectionService _connectionService;

        public StockIndexTickerService(IStockIndexTickerData stockIndexTickerData, IConnectionService connectionService)
        {
            _stockIndexTickerData = stockIndexTickerData;
            _connectionService = connectionService;
        }

        public async Task<int> AddStockIndex(StockIndexTicker stockTickerIndex)
        {
            var connection = _connectionService.GetConnectionToCommonShard();

            var indexId = await _stockIndexTickerData.AddStockIndex(connection, stockTickerIndex);

            _connectionService.DisposeConnection(connection);

            return indexId;
        }

        public async Task<bool> CheckIfStockTickerExists(string ticker)
        {
            var connection = _connectionService.GetConnectionToCommonShard();

            var tickerExists = await _stockIndexTickerData.CheckIfTickerExists(connection, ticker);

            _connectionService.DisposeConnection(connection);

            return tickerExists;
        }

        public async Task<StockIndexTicker> GetStockIndex(string ticker)
        {
            var connection = _connectionService.GetConnectionToCommonShard();

            var stockIndex = await _stockIndexTickerData.GetStockIndex(connection, ticker);

            _connectionService.DisposeConnection(connection);

            return stockIndex;
        }
    }
}
