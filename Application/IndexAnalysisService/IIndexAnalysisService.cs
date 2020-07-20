using Domain.DTO.StockAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.IndexAnalysisService
{
    public interface IIndexAnalysisService
    {
        Task<DailyIndexInvestmentOutcome> GetReturnsForDailyInvestment(ulong userId, IEnumerable<string> indexTickers);
        Task<DailyIndexInvestmentOutcome> GetReturnsForBimonthlyInvestment(ulong userId, IEnumerable<string> indexTickers);
    }
}
