using Domain.DTO.ExternalData;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService
{
    public class DailyStockPriceService : IDailyStockPriceService
    {
        public async Task<DailyStockPrice> GetDailyStockPriceService(string baseUrl, string ticker, string apiKey, DateTime startDate, DateTime endDate)
        {
            var formattedStartDate = $"{startDate.Year}-{startDate.Month}-{startDate.Day}";
            var formattedEndDate = $"{endDate.Year}-{endDate.Month}-{endDate.Day}";
            var completeUrl = $"{baseUrl}{ticker}?from={formattedStartDate}&to={formattedEndDate}&apikey={apiKey}";

            var client = new HttpClient();
            var stockDailyPriceJson = await client.GetStreamAsync(completeUrl);
            var dailyStockPrice = await JsonSerializer.DeserializeAsync<DailyStockPrice>(stockDailyPriceJson);

            return dailyStockPrice;
        }

    }
}
