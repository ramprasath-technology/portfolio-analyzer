using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.StockData
{
    public interface IStockData
    {
        Task<ulong> GetStockIdByTicker(string ticker, IDbConnection connection);
        Task<Stock> AddStock(IDbConnection connection, Stock stock);
        /// <summary>
        /// Gets stock data from stock_stock_data table
        /// </summary>
        /// <param name="connection">DB Connection</param>
        /// <param name="ticker">Ticker</param>
        /// <returns>Stock details</returns>
        Task<Stock> GetStockByTicker(IDbConnection connection, string ticker);
        Task<IEnumerable<Stock>> GetStocksById(IDbConnection connection, IEnumerable<ulong> stockId);

    }
}
