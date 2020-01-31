using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.HoldingsData
{
    public class HoldingsData : IHoldingsData
    {
        #region  SQL Statement 
        private const string _selAllCurrentHoldingsForUser =
            @"SELECT 
                *
            FROM
                portfolio_analyzer.stock_holding AS sh
                    INNER JOIN
                portfolio_analyzer.stock_stock_data AS ssd ON sh.stock_id = ssd.stock_id
                    INNER JOIN
                portfolio_analyzer.stock_stock_purchase AS ssp ON sh.purchase_id = ssp.purchase_id
                    INNER JOIN
                portfolio_analyzer.stock_stock_sale AS sss ON sh.sale_id = sss.sale_id
            WHERE
                sh.user_id = 0 AND sh.shard_number = 0";
        #endregion

        #region Methods
        public List<Holdings> GetAllHoldings(ulong userId, uint shardNumber)
        {
            return new List<Holdings>();

        }
        #endregion

    }
}
