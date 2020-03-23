using Dapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.StockPurchaseData
{
    public class StockPurchaseData : IStockPurchaseData
    {
        #region
        private const string insPurchaseData =
            @"INSERT INTO stock_stock_purchase(user_id,
                                 stock_id,
                                 quantity,
                                 price,
                                 date,
                                 comment)
                 VALUES (?userId,
                         ?stockId,
                         ?quantity,
                         ?price,
                         ?date,
                         ?comment);
            SELECT LAST_INSERT_ID();";
        #endregion

        public async Task<ulong> AddPurchase(Purchase purchase, IDbConnection connection)
        {
            connection.Open();

            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("userId", purchase.UserId);
            dynamicParams.Add("stockId", purchase.StockId);
            dynamicParams.Add("quantity", purchase.Quantity);
            dynamicParams.Add("price", purchase.Price);
            dynamicParams.Add("date", purchase.Date);
            dynamicParams.Add("comment", purchase.Comment);

            var purchaseId = await connection.QueryFirstAsync<ulong>(insPurchaseData, dynamicParams);

            connection.Close();

            return purchaseId;
        }


    }
}
