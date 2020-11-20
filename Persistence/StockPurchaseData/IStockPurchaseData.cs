using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.StockPurchaseData
{
    public interface IStockPurchaseData
    {
        Task<Purchase> AddPurchase(Purchase purchase, IDbConnection connection);
        Task<IEnumerable<Purchase>> GetPurchasesById(IDbConnection connection,
            IEnumerable<ulong> purchaseId);
        Task<IEnumerable<Purchase>> GetAllPurchasesForUser(IDbConnection connection, ulong userId);
        Task<IEnumerable<Purchase>> GetPurchasesByIdFilteredByDates(IDbConnection connection,
            IEnumerable<ulong> purchaseId,
            DateTime from,
            DateTime to);
        Task<IEnumerable<Purchase>> GetAllPurchasesByTicker(IDbConnection connection, ulong userId, string ticker);
        Task UpdatePurchasePriceAndQuantityByPurchaseId(IDbConnection connection,
            IEnumerable<Purchase> purchases);

    }
}
