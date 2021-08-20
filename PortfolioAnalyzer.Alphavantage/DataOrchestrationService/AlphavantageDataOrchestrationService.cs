using Domain.DTO.ExternalData;
using ExternalServices.OrchestrationService;
using PortfolioAnalyzer.Alphavantage.CompanyProfileService;
using PortfolioAnalyzer.AlphavantageAnalyzerService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PortfolioAnalyzer.Alphavantage.DataOrchestrationService
{
    public class AlphavantageDataOrchestrationService : IDataOrchestrationService
    {
        private readonly ICompanyProfileService _companyProfileService;
        private readonly IAlphaVantageAnalyzerService _alphaVantageAnalyzerService;

        public AlphavantageDataOrchestrationService(ICompanyProfileService companyProfileService,
            IAlphaVantageAnalyzerService alphaVantageAnalyzerService)
        {
            _companyProfileService = companyProfileService;
            _alphaVantageAnalyzerService = alphaVantageAnalyzerService;
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

        public async Task<IEnumerable<LastStockQuote>> GetLastStockQuotes(string baseUrl, string apiKey, IEnumerable<string> tickers)
        {
            var lastStockQuotes = new List<LastStockQuote>();
            var counter = 1;
            const int waitTime = 1000 * 60;
            foreach (var ticker in tickers)
            {
                var lastQuote = await Task.Run(() => _alphaVantageAnalyzerService.GetLatestQuoteForStock
                (
                    baseUrl, 
                    ticker, 
                    apiKey)
                );
                if (lastQuote.Price != 0m)
                {
                    lastStockQuotes.Add(lastQuote);
                }
                HandleThreadSleep(ref counter, waitTime);
            }

            return lastStockQuotes;
        }

        private void HandleThreadSleep(ref int counter, int waitTime)
        {
            if (counter % 5 == 0)
            {
                Thread.Sleep(waitTime);
            }
            counter++;
        }
    }
}
