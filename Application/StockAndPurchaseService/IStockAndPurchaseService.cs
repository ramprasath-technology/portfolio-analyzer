using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockAndPurchaseService
{
    public interface IStockAndPurchaseService
    {
        Task<StockPurchase> AddStockAndPurchaseInfo(StockPurchase stockPurchase);
    }
}
