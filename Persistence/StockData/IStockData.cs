using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.StockData
{
    public interface IStockData
    {
         Task<ulong> GetStockIdByTicker(string ticker, IDbConnection connection);
         Task<ulong> AddStock(Stock stock, IDbConnection connection);
    }
}
