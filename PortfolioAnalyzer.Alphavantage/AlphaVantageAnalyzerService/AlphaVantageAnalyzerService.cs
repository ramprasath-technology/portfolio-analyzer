using System.Text.Json;
using PortfolioAnalyzer.AlphavantageAnalyzerService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThreeFourteen.AlphaVantage;
using ThreeFourteen.AlphaVantage.Builders.Stocks;
using Domain.DTO.ExternalData;
using Newtonsoft.Json.Linq;

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
        



        public LastStockQuote GetLatestQuoteForStock(string url, string ticker, string apiKey)
        {
            var lastQuote = new LastStockQuote();
            var quoteInJsonFormat = GetLatestQuoteForStock(ticker, apiKey);
            ParseAndMapLastPriceForStock(quoteInJsonFormat.Result, lastQuote);
           /*
            var completeUrl = $"{url}&symbol={ticker}&apikey={apiKey}";
            var client = new System.Net.WebClient();
            dynamic jsonData = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(completeUrl));
            ParseGlobalQuote(jsonData);
           */
            return lastQuote;

        }



        private void ParseAndMapLastPriceForStock(string stockQuote,
            LastStockQuote lastQuote)
        {
            try
            {
                const string globalQuoteKey = "Global Quote";
                const string ticker = "01. symbol";
                const string price = "05. price";
                if (!string.IsNullOrEmpty(stockQuote))
                {
                    var response = JObject.Parse(stockQuote);
                    var globalQuote = response[globalQuoteKey].ToObject<Dictionary<string, string>>();
                    lastQuote.Symbol = globalQuote[ticker];
                    lastQuote.Price = Convert.ToDecimal(globalQuote[price]);
                }
            }
            catch (Exception ex)
            {
                //log exception and continue
            }
        }
    }
    
}
