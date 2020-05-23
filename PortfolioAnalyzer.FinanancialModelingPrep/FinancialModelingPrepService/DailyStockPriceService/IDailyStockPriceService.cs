using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService
{
    public interface IDailyStockPriceService
    {
        Task<DailyStockPrice> GetDailyStockPriceService(string baseUrl, string ticker, DateTime startDate, DateTime endDate);
    }
}
