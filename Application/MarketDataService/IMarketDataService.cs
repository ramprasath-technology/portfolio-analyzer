using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.MarketDataService
{
    public interface IMarketDataService
    {
        Task<DailyStockPrice> GetDailyStockPrice(string ticker, DateTime startDate, DateTime endDate);
        Task<IEnumerable<LastStockQuote>> GetLastStockQuote(IEnumerable<string> ticker);
    }
}
