using Dapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.StockData
{
    public class StockData : IStockData
    {
        #region Queries
        private const string insStock =
            @"INSERT INTO stock_stock_data(stock_ticker, company_name)
                VALUES (?ticker, ?companyName);
              SELECT LAST_INSERT_ID();";

        private const string selStockId =
            @"SELECT stock_id
                 FROM stock_stock_data s
                WHERE s.stock_ticker = ?ticker
                LIMIT 1;";

        private const string selStockByTicker =
            @"SELECT s.stock_id AS StockId,
                   s.stock_ticker AS Ticker,
                   s.company_name AS CompanyName
              FROM stock_stock_data s
             WHERE s.stock_ticker = ?ticker
             LIMIT 1;";

        private const string selStocksById =
            @"SELECT s.stock_id AS StockId,
                   s.stock_ticker AS Ticker,
                   s.company_name AS CompanyName
              FROM stock_stock_data s
             WHERE s.stock_id IN ?stockId;";
        #endregion

        public async Task<ulong> GetStockIdByTicker(string ticker, IDbConnection connection)
        {
            connection.Open();

            var parameters = new DynamicParameters();
            parameters.Add("ticker", ticker);

            var stockId = await connection.QueryFirstOrDefaultAsync<ulong>(selStockId, parameters);

            connection.Dispose();

            return stockId;            
        }    

        public async Task<Stock> GetStockByTicker(IDbConnection connection, string ticker)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ticker", ticker);

            connection.Open();

            var stock = await connection.QueryFirstOrDefaultAsync<Stock>(selStockByTicker, parameters);

            connection.Close();

            return stock;
        }

        public async Task<IEnumerable<Stock>> GetStocksById(IDbConnection connection, IEnumerable<ulong> stockId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("stockId", stockId);

            connection.Open();

            var stocks = await connection.QueryAsync<Stock>(selStocksById, parameters);

            connection.Close();

            return stocks;
        }

        public async Task<Stock> AddStock(IDbConnection connection, Stock stock)
        {
            connection.Open();

            var parameters = new DynamicParameters();
            parameters.Add("ticker", stock.Ticker);
            parameters.Add("companyName", stock.CompanyName);

            var stockId = await connection.QueryFirstAsync<ulong>(insStock, parameters);
            stock.StockId = stockId;

            connection.Dispose();

            return stock; ;
        }

    }
}
