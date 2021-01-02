using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockSaleService
{
    public interface IStockSaleService
    {
        Task<Sale> AddSale(ulong userId, Sale sale);
        Task<IEnumerable<Sale>> GetSalesByPurchaseIds(ulong userId,
            IEnumerable<ulong> purchaseIds,
            IDbConnection connection = null);
    }
}
