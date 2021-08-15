using Domain.DTO.ExternalData;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService.LastStockQuoteService
{
    public class LastStockQuoteService : ILastStockQuoteService
    {
        public async Task<IEnumerable<LastStockQuote>> GetLastQuoteForStocks(string baseUrl, string apiKey, IEnumerable<string> tickers)
        {
            try
            {
                var completeUrl = GetFullUrl(baseUrl, apiKey, tickers);

                var client = new HttpClient();
                var lastStockQuoteJson = await client.GetStreamAsync(completeUrl);
                var lastStockPrice = await JsonSerializer.DeserializeAsync<IEnumerable<LastStockQuote>>(lastStockQuoteJson);

                return lastStockPrice;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private string GetFullUrl(string baseUrl, string apiKey, IEnumerable<string> ticker)
        {
            const string separator = ",";
            var tickers = string.Join(separator, ticker);
            var fullUrl = $"{baseUrl}{tickers}?apikey={apiKey}";

            return fullUrl;
        }
    }
}
