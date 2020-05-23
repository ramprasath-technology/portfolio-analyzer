﻿using Dapper;
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

        private const string selPurchasesById =
            @"SELECT s.comment AS Comment,
                   s.`date` AS Date,
                   s.price AS Price,
                   s.purchase_id AS PurchaseId,
                   s.stock_id AS StockId,
                   s.quantity AS Quantity,
                   s.user_id AS UserId
              FROM stock_stock_purchase s
             WHERE s.purchase_id IN ?purchaseId;";
                    #endregion

        public async Task<Purchase> AddPurchase(Purchase purchase, IDbConnection connection)
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
            purchase.PurchaseId = purchaseId;

            connection.Close();

            return purchase;
        }

        public async Task<IEnumerable<Purchase>> GetPurchasesById(IDbConnection connection,
            IEnumerable<ulong> purchaseId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("purchaseId", purchaseId);

            connection.Open();
            var purchases = await connection.QueryAsync<Purchase>(selPurchasesById, parameters);
            connection.Close();

            return purchases;
        }


    }
}
