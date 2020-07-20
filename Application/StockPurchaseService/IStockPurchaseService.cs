using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockPurchaseService
{
    public interface IStockPurchaseService
    {
        Task<Purchase> AddStockPurchase(ulong userId, Purchase purchase);
        Task<IEnumerable<Purchase>> GetPurchasesById(ulong userId, IEnumerable<ulong> purchaseId);
        Task<IEnumerable<Purchase>> GetPurchasesForUser(ulong userId);
        Task<IEnumerable<Purchase>> GetPurchasesByIdFilteredByDates(ulong userId,
            IEnumerable<ulong> purchaseId,
            DateTime from,
            DateTime to);
    }
}
