using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO.ExternalData;

namespace Application.MarketDataService
{
    public interface IMarketDataService
    {
        Task<DailyStockPrice> GetDailyStockPrice(string ticker, DateTime startDate, DateTime endDate);
        Task<IEnumerable<LastStockQuote>> GetLastStockQuote(IEnumerable<string> ticker);
        Task<CompanyProfile> GetCompanyProfile(string ticker);
    }
}
