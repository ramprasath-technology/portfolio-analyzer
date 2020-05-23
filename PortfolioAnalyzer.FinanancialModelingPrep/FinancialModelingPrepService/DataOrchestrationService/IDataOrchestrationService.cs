using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService.DataOrchestrationService
{
    public interface IDataOrchestrationService
    {
        Task<CompanyProfile> GetCompanyProfile(string url, string apiKey, string ticker);
        Task<DailyStockPrice> GetDailyStockPriceService(string baseUrl, string ticker, DateTime startDate, DateTime endDate);
        Task<IEnumerable<LastStockQuote>> GetLastStockQuotes(string baseUrl, string apiKey, IEnumerable<string> ticker);
    }
}
