using Application.ConfigService;
using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepDTO;
using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService;
using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService.DataOrchestrationService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.MarketDataService
{
    public class MarketDataService : IMarketDataService
    {
        private readonly IDataOrchestrationService _dataOrchestrationService;
        private readonly IConfigService _configService;

        public MarketDataService(IDataOrchestrationService dataOrchestrationService, IConfigService configService)
        {
            _dataOrchestrationService = dataOrchestrationService;
            _configService = configService;
        }

        public async Task<DailyStockPrice> GetDailyStockPrice(string ticker, DateTime startDate, DateTime endDate)
        {
            var baseUrl = _configService.GetDailyPriceUrl();
            var apiKey = _configService.GetFinancialModelingPrepKey();

            var dailyStockPrice = await _dataOrchestrationService.GetDailyStockPriceService(baseUrl, ticker, apiKey, startDate.Date, endDate.Date);

            return dailyStockPrice;
        }

        public async Task<IEnumerable<LastStockQuote>> GetLastStockQuote(IEnumerable<string> ticker)
        {
            var baseUrl = _configService.GetStockQuoteUrl();
            var apiKey = _configService.GetFinancialModelingPrepKey();

            var lastStockQuote = await _dataOrchestrationService.GetLastStockQuotes(baseUrl, apiKey, ticker);

            return lastStockQuote;
        }
    }
}
