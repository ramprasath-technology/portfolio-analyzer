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
        Task<ulong> AddPurchase(Purchase purchase, IDbConnection connection);

    }
}
