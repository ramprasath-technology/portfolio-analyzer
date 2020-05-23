using Domain.DTO.StockAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockIndexComparisonService
{
    public interface IStockIndexComparisonService
    {
        Task<IEnumerable<StockComparisonToIndex>> GetComparisonWithIndex(ulong userId, IEnumerable<string> indexTickers);
    }
}
