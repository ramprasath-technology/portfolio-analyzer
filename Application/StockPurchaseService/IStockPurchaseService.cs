using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockPurchaseService
{
    public interface IStockPurchaseService
    {
        Task<ulong> AddStockPurchase(ulong userId, Purchase purchase);
    }
}
