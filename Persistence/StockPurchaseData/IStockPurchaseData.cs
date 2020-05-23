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

    }
}
