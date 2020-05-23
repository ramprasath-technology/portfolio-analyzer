using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepDTO;
using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService.LastStockQuoteService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService.DataOrchestrationService
{
    public class DataOrchestrationService : IDataOrchestrationService
    {
        private readonly ICompanyProfileService _companyProfileService;
        private readonly IDailyStockPriceService _dailyStockPriceService;
        private readonly ILastStockQuoteService _lastStockQuoteService;

        public DataOrchestrationService(ICompanyProfileService companyProfileService, 
            IDailyStockPriceService dailyStockPriceService,
            ILastStockQuoteService lastStockQuoteService)
        {
            _companyProfileService = companyProfileService;
            _dailyStockPriceService = dailyStockPriceService;
            _lastStockQuoteService = lastStockQuoteService;
        }

        public async Task<CompanyProfile> GetCompanyProfile(string url, string apiKey, string ticker)
        {
            var companyProfile = await _companyProfileService.GetCompanyProfile(url, apiKey, ticker);

            return companyProfile;
        }

        public async Task<DailyStockPrice> GetDailyStockPriceService(string baseUrl, string ticker, DateTime startDate, DateTime endDate)
        {
            var dailyStockPrice = await _dailyStockPriceService.GetDailyStockPriceService(baseUrl, ticker, startDate, endDate);

            return dailyStockPrice;
        }

        public async Task<IEnumerable<LastStockQuote>> GetLastStockQuotes(string baseUrl, string apiKey, IEnumerable<string> ticker)
        {
            var lastStockQuote = await _lastStockQuoteService.GetLastQuoteForStocks(baseUrl, apiKey, ticker);

            return lastStockQuote;
        }


    }
}
