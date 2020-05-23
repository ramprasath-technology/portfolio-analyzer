using Dapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.StockSaleData
{
    public class StockSaleData : IStockSaleData
    {
        #region
        private const string insStockSale =
            @"INSERT INTO stock_stock_sale(user_id,
                             stock_id,
                             quantity,
                             price,
                             `date`,
                             comment,
                             purchase_id,
                             date_added,
                             username)
             VALUES (?userId,
                     ?stockId,
                     ?quantity,
                     ?price,
                     ?date,
                     ?comment,
                     ?purchaseId,
                     ?dateAdded,
                     ?userName);
                    SELECT LAST_INSERT_ID();";
        #endregion

        public async Task<Sale> AddSale(IDbConnection connection, Sale sale)
        {
            var parameters = new DynamicParameters();
            parameters.Add("userId", sale.UserId);
            parameters.Add("stockId", sale.StockId);
            parameters.Add("quantity", sale.Quantity);
            parameters.Add("price", sale.Price);
            parameters.Add("date", sale.Date);
            parameters.Add("comment", sale.Comment);
            parameters.Add("purchaseId", sale.PurchaseId);
            parameters.Add("dateAdded", sale.DateAdded);
            parameters.Add("username", sale.Username);

            connection.Open();
            var saleId = await connection.QueryFirstAsync<ulong>(insStockSale, parameters);
            connection.Close();

            sale.SaleId = saleId;

            return sale;
        }
    }
}
