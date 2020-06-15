using Domain.DTO.StockAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockTotalValueService
{
    public interface IStockTotalValueService
    {
        Task<TotalHoldingsValue> GetTotalValueForUserHolding(ulong userId);
    }
}
