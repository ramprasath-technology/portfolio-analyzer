using Dapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.HoldingsData
{
    public class HoldingsData : IHoldingsData
    {
        #region  SQL Statement 
        private const string _selAllHoldingsForUser =
            @"SELECT *
              FROM stock_holding s
             WHERE s.user_id = ?userId;";

        private const string _selAllHoldingsForUserWithStockDetails =
            @"   SELECT s.holding_id AS HoldingId,
                   s.user_id AS UserId,
                   s.stock_id AS StockId,
                   s.holding_details AS HoldingDetails,
                   s1.stock_ticker AS Ticker,
                   s1.company_name AS CompanyName
              FROM stock_holding s
                   INNER JOIN stock_stock_data s1 ON s.stock_id = s1.stock_id
              WHERE s.user_id = ?userId; ";

        private const string _selHoldingDetailsForParticularStock =
            @"SELECT s.holding_id, s.holding_details
              FROM stock_holding s
             WHERE s.user_id = ?userId AND s.stock_id = ?stockId;";

        private const string _updHoldingDetails =
            @"UPDATE stock_holding
               SET holding_details = ?holdingDetails
             WHERE holding_id = ?holdingId;
             SELECT LAST_INSERT_ID();";

        private const string _insHolding =
            @"INSERT INTO stock_holding(user_id,
                          stock_id,
                          holding_details)
                 VALUES (?userId,
                         ?stockId,
                         ?holdingDetails);
            SELECT LAST_INSERT_ID();";

        private const string _chkIfHoldingExists =
            @"SELECT EXISTS
              (SELECT 1
                 FROM stock_holding s
                WHERE s.user_id = ?userId AND s.stock_id = ?stockId);";

        private const string _delStockHolding =
            @"DELETE FROM stock_holding
                WHERE holding_id = ?holdingId;";
        #endregion

        #region Methods
        public async Task<IEnumerable<Holdings>> GetAllHoldingsForUser(IDbConnection connection, ulong userId)
        {
            var userHoldings = new List<Holdings>();
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("userId", userId);

            connection.Open();
            var holdings = await connection.QueryAsync(_selAllHoldingsForUser, dynamicParameters);
            connection.Close();

            foreach(var holdingData in holdings)
            {
                var holdingId = holdingData.holding_id;
                var holdingDetails = holdingData.holding_details;
                var stockId = holdingData.stock_id;
                var deserializedHoldingDetails = JsonSerializer.Deserialize<IEnumerable<HoldingDetails>>(holdingDetails);

                var holding = new Holdings()
                {
                    HoldingDetails = deserializedHoldingDetails,
                    HoldingId = holdingId,
                    UserId = userId,
                    StockId = stockId
                };
                userHoldings.Add(holding);
            }

            return userHoldings;

        }

        public async Task<IEnumerable<Holdings>> GetAllHoldingsForUserWithStockDetails(IDbConnection connection, ulong userId)
        {
            var holdings = new List<Holdings>();
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("userId", userId);

            var userHoldings = await connection.QueryAsync(_selAllHoldingsForUserWithStockDetails, dynamicParameters);

            foreach (var userHolding in userHoldings)
            {
                var holdingDetails = userHolding.HoldingDetails;
                var deserializedHoldingDetails = JsonSerializer.Deserialize<IEnumerable<HoldingDetails>>(holdingDetails);

                var stock = new Stock()
                {
                    StockId = userHolding.StockId,
                    CompanyName = userHolding.CompanyName,
                    Ticker = userHolding.Ticker
                };

                var holding = new Holdings()
                {
                    HoldingDetails = deserializedHoldingDetails,
                    HoldingId = userHolding.HoldingId,
                    StockId = userHolding.StockId,
                    UserId = userHolding.UserId,
                    Stock = stock
                };

                holdings.Add(holding);
            }
           
            return holdings;
        }


        public async Task<Holdings> GetHoldingDataForUserAndStock(IDbConnection connection, ulong userId, ulong stockId)
        {
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("userId", userId);
            dynamicParameters.Add("stockId", stockId);

            connection.Open();
            var holdingData = await connection.QueryFirstAsync(_selHoldingDetailsForParticularStock, dynamicParameters);
            connection.Close();

            var holdingId = holdingData.holding_id;
            var holdingDetails = holdingData.holding_details;

            var deserializedHoldingDetails = JsonSerializer.Deserialize<IEnumerable<HoldingDetails>>(holdingDetails);

            var holding = new Holdings()
            {
                HoldingDetails = deserializedHoldingDetails,
                HoldingId = holdingId,
                StockId = stockId,
                UserId = userId
            };

            return holding;
        }

        public async Task UpdateHoldingDetail(IDbConnection connection, ulong holdingId, IEnumerable<HoldingDetails> holdingDetails)
        {
            var serializedHoldingDetails = JsonSerializer.Serialize(holdingDetails);

            var parameters = new DynamicParameters();
            parameters.Add("holdingDetails", serializedHoldingDetails);
            parameters.Add("holdingId", holdingId);          

            connection.Open();
            await connection.ExecuteAsync(_updHoldingDetails, parameters);
            connection.Close();
        }

        public async Task<ulong> AddHolding(IDbConnection connection, Holdings holdings)
        {
            var serializedHoldingDetails = JsonSerializer.Serialize(holdings.HoldingDetails);

            var parameters = new DynamicParameters();
            parameters.Add("userId", holdings.UserId);
            parameters.Add("stockId", holdings.StockId);
            parameters.Add("holdingDetails", serializedHoldingDetails);

            connection.Open();
            var holdingId = await connection.QueryFirstAsync<ulong>(_insHolding, parameters);
            connection.Close();

            return holdingId;
        }

        public async Task<bool> CheckIfHoldingExists(IDbConnection connection, ulong userId, ulong stockId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("userId", userId);
            parameters.Add("stockId", stockId);

            connection.Open();
            var doesHoldingExists = await connection.QueryFirstAsync<bool>(_chkIfHoldingExists, parameters);
            connection.Close();

            return doesHoldingExists;
        }

        public async Task DeleteHolding(IDbConnection connection, ulong holdingId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("holdingId", holdingId);

            connection.Open();
            await connection.ExecuteAsync(_delStockHolding, parameters);
            connection.Close();
        }

        #endregion

    }
}
