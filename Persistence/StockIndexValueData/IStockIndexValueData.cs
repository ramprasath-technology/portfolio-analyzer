using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.StockIndexValueData
{
    public interface IStockIndexValueData
    {
        /// <summary>
        /// Get ticker value for the last date available in stock_index_value table
        /// </summary>
        /// <param name="connection">DB Connection</param>
        /// <param name="ticker">Ticker</param>
        /// <returns>Stock Index Value</returns>
        Task<StockIndexValue> GetLastValueForTicker(IDbConnection connection, string ticker);
        /// <summary>
        /// Add daily values for a given stock market index into stock_index_value table
        /// </summary>
        /// <param name="connection">DB Connection</param>
        /// <param name="stockIndexValue">Collection of Stock Index Values</param>
        /// <returns></returns>
        Task AddIndexValue(IDbConnection connection, IEnumerable<StockIndexValue> stockIndexValue);
        Task<int> GetNumberOfDaysTickerValueIsPresent(IDbConnection connection, int tickerId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<StockIndexValue>> GetPricesForGivenIndexTickersAndDates(IDbConnection connection,
            IEnumerable<string> ticker,
            IEnumerable<DateTime> date);
    }
}
