using Domain.DTO.ExternalData;
using ExternalServices.OrchestrationService;
using PortfolioAnalyzer.Alphavantage.CompanyProfileService;
using PortfolioAnalyzer.AlphavantageAnalyzerService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAnalyzer.Alphavantage.DataOrchestrationService
{
    public class AlphavantageDataOrchestrationService : IDataOrchestrationService
    {
        private readonly ICompanyProfileService _companyProfileService;
        public AlphavantageDataOrchestrationService(ICompanyProfileService companyProfileService)
        {
            _companyProfileService = companyProfileService;
        }

        public async Task<CompanyProfile> GetCompanyProfile(string url, string apiKey, string ticker)
        {
            var companyProfile = await Task.Run(() => _companyProfileService.GetCompanyProfile
            (
                url, 
                apiKey, 
                ticker)
            );
            return companyProfile;
        }

        public Task<DailyStockPrice> GetDailyStockPriceService(string baseUrl, string ticker, string apiKey, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LastStockQuote>> GetLastStockQuotes(string baseUrl, string apiKey, IEnumerable<string> ticker)
        {
            throw new NotImplementedException();
        }
    }
}
