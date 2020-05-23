using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Connection;
using Application.MarketDataService;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PortfolioAnalyzer.AlphavantageAnalyzerService;

namespace Application.StockQuoteService
{
    public class StockQuoteService : IStockQuoteService
    {
        private readonly IAlphaVantageAnalyzerService _alphaVantageAnalyzerService;
        private readonly IConnectionService _connectionService;
        private readonly IMarketDataService _marketDataService;

        public StockQuoteService(IAlphaVantageAnalyzerService alphaVantageAnalyzerService, 
            IConnectionService connectionService,
            IMarketDataService marketDataService)
        {
            _alphaVantageAnalyzerService = alphaVantageAnalyzerService;
            _connectionService = connectionService;
            _marketDataService = marketDataService;
        }

        public async Task<Dictionary<string, decimal>> GetLatestQuoteForStocks(IEnumerable<string> tickers)
        {
            var tickerPriceMapping = new Dictionary<string, decimal>();
            var alphaVantageKey = _connectionService.GetAlphaVantageAPIKey();
            //List<string> tickerList = tickers.ToList();
            var latestQuoteTasks = new Task<string>[tickers.Count()];

            Parallel.ForEach(tickers, (ticker, state, index) =>
            {
                latestQuoteTasks[index] = _alphaVantageAnalyzerService.GetLatestQuoteForStock(ticker, alphaVantageKey);
            });

            await Task.WhenAll(latestQuoteTasks);

            Parallel.ForEach(latestQuoteTasks, (quoteTask, state, index) =>
            {
                var response = quoteTask.Result;
                ParseLastPriceForStock(response, tickerPriceMapping);
            });

            return tickerPriceMapping;
        }

        private Dictionary<string, string> ParseLastPriceForStock(string stockQuote, Dictionary<string, decimal> tickerPriceMapping)
        {
            const string globalQuoteKey = "Global Quote";
            const string ticker = "01. symbol";
            const string price = "05. price";

            var response = JObject.Parse(stockQuote);
            var globalQuote = response[globalQuoteKey].ToObject<Dictionary<string, string>>();
            tickerPriceMapping[globalQuote[ticker]] = Convert.ToDecimal(globalQuote[price]);

            return globalQuote;
        }
    }
}
