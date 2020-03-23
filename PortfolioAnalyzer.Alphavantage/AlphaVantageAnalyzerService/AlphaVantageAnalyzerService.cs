using PortfolioAnalyzer.AlphavantageAnalyzerService;
using System;
using System.Threading.Tasks;
using ThreeFourteen.AlphaVantage;
using ThreeFourteen.AlphaVantage.Builders.Stocks;

namespace PortfolioAnalyzer.AlphavantageAnalyzerService
{
    public class AlphaVantageAnalyzerService : IAlphaVantageAnalyzerService
    {
        public Task<string> GetLatestQuoteForStock(string ticker, string apiKey)
        {
            var alphaVantage = new AlphaVantage(apiKey);
            var latestStockQuote = alphaVantage.Custom()
                .Set("symbol", ticker)
                .Set("function", "GLOBAL_QUOTE")
                .GetRawDataAsync();

            return latestStockQuote;
        }

        /*public void GetLatestQuoteForStock(string ticker)
        {
            var client = new Http
        }*/
    }
    
}
