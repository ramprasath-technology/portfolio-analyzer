using Domain.DTO.StockAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.IndividualStockComparisonService
{
    public interface IIndividualStockComparisonService
    {
        Task<IEnumerable<IndividualStockReturn>> GetIndividualComparisonService(ulong userId);
    }
}
