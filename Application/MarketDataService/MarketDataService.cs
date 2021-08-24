using Application.ConfigService;
using Domain.DTO.ExternalData;
using ExternalServices.OrchestrationService;
using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService;
using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService.DataOrchestrationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.ConfigService.AlphaVantageServiceConfig;
using static PortfolioAnalyzer.Alphavantage.DataOrchestrationService.AlphavantageDataOrchestrationService;

namespace Application.MarketDataService
{
    public class MarketDataService : IMarketDataService
    {
        public delegate IDataOrchestrationService ExternalServiceResolver(string key);
        public delegate IExternalServiceConfig ExternalServiceConfigResolver(string key);
        private readonly IDataOrchestrationService _dataOrchestrationService;
        private readonly IExternalServiceConfig _externalServiceConfig;
        private readonly IConfigService _configService;

        public MarketDataService(ExternalServiceResolver serviceResolver, 
            IConfigService configService,
            ExternalServiceConfigResolver externalServiceConfig)
        {
            _configService = configService;
            _dataOrchestrationService = serviceResolver(_configService.GetExternalDataServiceName());
            _externalServiceConfig = externalServiceConfig(_configService.GetExternalDataServiceName());
            
        }

        public async Task<DailyStockPrice> GetDailyStockPrice(string ticker, DateTime startDate, DateTime endDate)
        {
            var baseUrl = _externalServiceConfig.GetDailyPriceUrl();
            var apiKey = _externalServiceConfig.GetKey();

            var dailyStockPrice = await _dataOrchestrationService.GetDailyStockPriceService(baseUrl, ticker, apiKey, startDate.Date, endDate.Date);

            return dailyStockPrice;
        }

        public async Task<IEnumerable<LastStockQuote>> GetLastStockQuote(IEnumerable<string> tickers)
        {
            var baseUrl = _externalServiceConfig.GetStockQuoteUrl();
            var apiKey = _externalServiceConfig.GetKey();
            var lastStockQuote = await _dataOrchestrationService.GetLastStockQuotes(baseUrl, apiKey, tickers);

            return lastStockQuote;
        }

        public async Task<CompanyProfile> GetCompanyProfile(string ticker)
        {
            var baseUrl = _externalServiceConfig.GetCompanyProfileUrl();
            var apiKey = _externalServiceConfig.GetKey();

            var companyProfile = await _dataOrchestrationService.GetCompanyProfile(baseUrl, apiKey, ticker);

            return companyProfile;
        }
    }
}
