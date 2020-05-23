using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.StockSaleData
{
    public interface IStockSaleData
    {
        Task<Sale> AddSale(IDbConnection connection, Sale sale);
    }
}
