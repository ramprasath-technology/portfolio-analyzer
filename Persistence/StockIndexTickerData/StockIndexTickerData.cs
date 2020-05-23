using Dapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.StockIndexTickerData
{
    public class StockIndexTickerData : IStockIndexTickerData
    {
        #region Queries

        private readonly string insStockIndexTicker =
            @"INSERT INTO stock_index_ticker(ticker, ticker_description)
                VALUES (?ticker, ?tickerDescription);
                SELECT LAST_INSERT_ID();";

        private readonly string chkStockIndexTicker =
            @"SELECT EXISTS
              (SELECT 1
                 FROM stock_index_ticker s
                WHERE s.ticker = ?ticker);";

        private readonly string selStockTicker =
            @"SELECT s.ticker_id AS TickerId,
                   s.ticker AS Ticker,
                   s.ticker_description AS TickerDescription
             FROM stock_index_ticker s
             WHERE s.ticker = ?ticker;";

        #endregion

        public async Task<int> AddStockIndex(IDbConnection connection, StockIndexTicker stockIndexTicker)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ticker", stockIndexTicker.Ticker);
            parameters.Add("tickerDescription", stockIndexTicker.TickerDescription);

            connection.Open();
            var tickerId = await connection.QueryFirstAsync<int>(insStockIndexTicker, parameters);
            connection.Close();

            return tickerId;
        }

        public async Task<StockIndexTicker> GetStockIndex(IDbConnection connection, string ticker)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ticker", ticker);

            connection.Open();
            var tickerDetails = await connection.QueryFirstAsync<StockIndexTicker>(selStockTicker, parameters);
            connection.Close();

            return tickerDetails;
        }

        public async Task<bool> CheckIfTickerExists(IDbConnection connection, string ticker)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ticker", ticker);

            connection.Open();
            var tickerExists = await connection.ExecuteScalarAsync<bool>(chkStockIndexTicker, parameters);
            connection.Close();

            return tickerExists;
        }
    }
}
