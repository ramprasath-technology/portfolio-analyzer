using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.StockHoldingService
{
    public interface IStockHoldingService
    {
        Task<IEnumerable<Holdings>> GetAllHoldingsForUser(ulong userId);
        /// <summary>
        /// Add holding details from purchase data
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="purchase">Purchase data</param>
        /// <returns></returns>
        Task AddPurchaseToHoldings(ulong userId, Purchase purchase);
        Task UpdateHoldingAfterSale(ulong userId, Sale sale);
        Task<IEnumerable<Holdings>> GetAllHoldingsForUserWithStockDetails(ulong userId);
        IEnumerable<ulong> GetPurchaseIdsFromHolding(IEnumerable<Holdings> holdings);
    }
}
