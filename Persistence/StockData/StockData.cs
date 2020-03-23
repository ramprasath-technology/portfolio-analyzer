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
            @"INSERT INTO stock_stock_data(stock_ticker, company_name, shard_number)
                VALUES (?ticker, ?companyName, ?shardNumber);
              SELECT LAST_INSERT_ID();";

        private const string selStockId =
            @"SELECT stock_id
                 FROM stock_stock_data s
                WHERE s.stock_ticker = ?ticker
                LIMIT 1;";
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

        public async Task<ulong> AddStock(Stock stock, IDbConnection connection)
        {
            connection.Open();

            var parameters = new DynamicParameters();
            parameters.Add("ticker", stock.Ticker);
            parameters.Add("companyName", stock.CompanyName);
            parameters.Add("shardNumber", stock.ShardNumber);

            var stockId = await connection.QueryFirstAsync<ulong>(insStock, parameters);

            connection.Dispose();

            return stockId;
        }

    }
}
