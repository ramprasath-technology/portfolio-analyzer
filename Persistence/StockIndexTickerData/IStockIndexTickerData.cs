using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.StockIndexTickerData
{
    public interface IStockIndexTickerData
    {
        Task<int> AddStockIndex(IDbConnection connection, StockIndexTicker stockIndexTicker);
        /// <summary>
        /// Check if an index ticker exists in the stock_index_ticker table
        /// </summary>
        /// <param name="connection">DB Connection</param>
        /// <param name="ticker">Ticker</param>
        /// <returns>True or False</returns>
        Task<bool> CheckIfTickerExists(IDbConnection connection, string ticker);
        /// <summary>
        /// Get stock index ticker record from stock_index_ticker table using ticker
        /// </summary>
        /// <param name="connection">DB Connection</param>
        /// <param name="ticker">Ticker</param>
        /// <returns>Stock Index Ticker row</returns>
        Task<StockIndexTicker> GetStockIndex(IDbConnection connection, string ticker);
    }
}
