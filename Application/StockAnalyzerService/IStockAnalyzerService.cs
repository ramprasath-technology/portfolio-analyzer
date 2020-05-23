using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockAnalyzerService
{
    public interface IStockAnalyzerService
    {
        Task<HoldingsSP500Mapping[]> GetComparisonWithSP500(ulong userId);
    }
}
