using Domain.DTO.StockAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockReturnsService
{
    public interface IStockReturnsService
    {
        Task<IEnumerable<StockAnnualizedReturn>> GetAnnualizedReturnForCurrentHoldings(ulong userId);
    }

}
