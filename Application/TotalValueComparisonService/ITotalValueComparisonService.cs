using Domain.DTO.StockAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.TotalValueComparisonService
{
    public interface ITotalValueComparisonService
    {
        Task<TotalValueComparisonToIndex> GetTotalValueComparison(ulong userId, IEnumerable<string> indexTickers);
    }
}
