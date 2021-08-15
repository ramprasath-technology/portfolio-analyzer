using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockPurchaseService
{
    public interface IStockPurchaseService
    {
        Task<Purchase> AddStockPurchase(ulong userId, Purchase purchase);
        Task<IEnumerable<Purchase>> GetPurchasesById(ulong userId, IEnumerable<ulong> purchaseId);
        Task<IEnumerable<Purchase>> GetPurchasesForUser(ulong userId,
            IDbConnection connection = null);
        Task<IEnumerable<Purchase>> GetPurchasesByIdFilteredByDates(ulong userId,
            IEnumerable<ulong> purchaseId,
            DateTime from,
            DateTime to);
        Task<IEnumerable<Purchase>> GetAllPurchasesForTicker(ulong userId, string ticker);
        Task UpdatePurchasePriceAndQuantityByPurchaseId(ulong userId, IEnumerable<Purchase> purchases);
        Task<IEnumerable<Purchase>> GetPurchasesForUserWithStockData(ulong userId,
            IDbConnection connection = null);
        Task<Purchase> GetLastPurchaseForUser(ulong userId, IDbConnection connection = null);
    }
}
