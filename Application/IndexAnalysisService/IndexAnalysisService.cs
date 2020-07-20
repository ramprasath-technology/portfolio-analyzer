using Application.MarketDataService;
using Application.StockHoldingService;
using Application.StockIndexValueService;
using Application.StockPurchaseService;
using Domain;
using Domain.DTO.StockAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IndexAnalysisService
{
    public class IndexAnalysisService : IIndexAnalysisService
    {
        private readonly IStockPurchaseService _stockPurchaseService;
        private readonly IStockIndexValueService _stockIndexValueService;
        private readonly IStockHoldingService _stockHoldingService;
        private readonly IMarketDataService _marketDataService;

        public IndexAnalysisService(IStockPurchaseService stockPurchaseService, 
            IStockIndexValueService stockIndexValueService,
            IStockHoldingService stockHoldingService,
            IMarketDataService marketDataService)
        {
            _stockPurchaseService = stockPurchaseService;
            _stockIndexValueService = stockIndexValueService;
            _stockHoldingService = stockHoldingService;
            _marketDataService = marketDataService;
        }

        public async Task<DailyIndexInvestmentOutcome> GetReturnsForDailyInvestment(ulong userId, IEnumerable<string> indexTickers)
        {
            var outcome = new DailyIndexInvestmentOutcome();
            var tickerValueMap = new Dictionary<string, decimal>();
            var ticketReturnPercentageMap = new Dictionary<string, decimal>();
            var purchaseTask = _stockPurchaseService.GetPurchasesForUser(userId);
            var holdingTask = _stockHoldingService.GetAllHoldingsForUser(userId);
            var purchases = await purchaseTask;
            var holdings = await holdingTask;
            var totalCapitalSpent = holdings.Sum(x => x.HoldingDetails.Sum(y => y.Price * y.Quantity));

            if (purchases.Count() > 0)
            {
                var currentIndexPricesTask = _marketDataService.GetLastStockQuote(indexTickers);
                var firstPurchaseDate = purchases.OrderBy(x => x.Date).First();
                var dates = GetDatesForAddingInvestment(firstPurchaseDate.Date);
                var perDayInvestment = totalCapitalSpent / dates.Count();                
                var prices = await _stockIndexValueService.GetPricesForGivenIndexTickersAndDates(userId, indexTickers, dates);
                var tickerUnits = CalculateHypotheticalPurchaseQuantity(perDayInvestment, indexTickers, dates, prices);
                var currentIndexPrices = await currentIndexPricesTask;

                foreach (var indexPrice in currentIndexPrices)
                {
                    tickerValueMap[indexPrice.Symbol] = Math.Round(indexPrice.Price * tickerUnits[indexPrice.Symbol], 2);
                    ticketReturnPercentageMap[indexPrice.Symbol] = Math.Round(((tickerValueMap[indexPrice.Symbol] - totalCapitalSpent) / totalCapitalSpent) * 100, 2);
                }                
   
                outcome.HypotheticalCurrentValues = tickerValueMap;
                outcome.InvestmentPerDay = perDayInvestment;
                outcome.TotalInvestment = totalCapitalSpent;
                outcome.TotalNumberOfDays = dates.Count();
                outcome.HypotheticalReturnPercentage = ticketReturnPercentageMap;
            }           

            return outcome;                      
        }

        public async Task<DailyIndexInvestmentOutcome> GetReturnsForBimonthlyInvestment(ulong userId, IEnumerable<string> indexTickers)
        {
            var outcome = new DailyIndexInvestmentOutcome();
            var tickerValueMap = new Dictionary<string, decimal>();
            var ticketReturnPercentageMap = new Dictionary<string, decimal>();
            var purchaseTask = _stockPurchaseService.GetPurchasesForUser(userId);
            var holdingTask = _stockHoldingService.GetAllHoldingsForUser(userId);
            var purchases = await purchaseTask;
            var holdings = await holdingTask;
            var totalCapitalSpent = holdings.Sum(x => x.HoldingDetails.Sum(y => y.Price * y.Quantity));

            if (purchases.Count() > 0)
            {
                var currentIndexPricesTask = _marketDataService.GetLastStockQuote(indexTickers);
                var firstPurchase = purchases.OrderBy(x => x.Date).First();
                var dates = GetDatesForBimonthlyInvestment(firstPurchase.Date);
                var datesForIndexValues = GetAllDaysBetweenDates(firstPurchase.Date, DateTime.Now);
                var perDayInvestment = totalCapitalSpent / dates.Count();
                var prices = await _stockIndexValueService.GetPricesForGivenIndexTickersAndDates(userId, indexTickers, datesForIndexValues);
                var tickerUnits = CalculateHypotheticalPurchaseQuantity(perDayInvestment, indexTickers, dates, prices);
                var currentIndexPrices = await currentIndexPricesTask;

                foreach (var indexPrice in currentIndexPrices)
                {
                    tickerValueMap[indexPrice.Symbol] = Math.Round(indexPrice.Price * tickerUnits[indexPrice.Symbol], 2);
                    ticketReturnPercentageMap[indexPrice.Symbol] = Math.Round(((tickerValueMap[indexPrice.Symbol] - totalCapitalSpent) / totalCapitalSpent) * 100, 2);
                }

                outcome.HypotheticalCurrentValues = tickerValueMap;
                outcome.InvestmentPerDay = perDayInvestment;
                outcome.TotalInvestment = totalCapitalSpent;
                outcome.TotalNumberOfDays = dates.Count();
                outcome.HypotheticalReturnPercentage = ticketReturnPercentageMap;
            }

            return outcome;
        }

        private IEnumerable<DateTime> GetDatesForAddingInvestment(DateTime firstPurchaseDate)
        {
            var dates = new List<DateTime>();
            dates.Add(firstPurchaseDate);

            var dateTracker = firstPurchaseDate.AddDays(1);
             
            while (dateTracker.Date <= DateTime.Now.Date)
            {
                //This does not take into account market holidays
                if (dateTracker.DayOfWeek != DayOfWeek.Saturday && dateTracker.DayOfWeek != DayOfWeek.Sunday)
                {
                    dates.Add(dateTracker);                   
                }
                dateTracker = dateTracker.AddDays(1);
            }

            return dates;
        }

        private IEnumerable<DateTime> GetAllDaysBetweenDates(DateTime start, DateTime end)
        {
            var dates = new List<DateTime>();
            while (start.Date <= end.Date)
            {
                dates.Add(start);
                start = start.AddDays(1);
            }

            return dates;
        }

        private IEnumerable<DateTime> GetDatesForBimonthlyInvestment(DateTime firstPurchaseDate)
        {
            var dates = new List<DateTime>();
            var firstDay = firstPurchaseDate.Day;
            DateTime investmentDate = DateTime.MinValue;

            if (firstDay < 15)
            {
                investmentDate = new DateTime(firstPurchaseDate.Year, firstPurchaseDate.Month, 15);
            }
            else
            {
                investmentDate = new DateTime(firstPurchaseDate.Year, firstPurchaseDate.Month, 1);
                investmentDate = investmentDate.AddMonths(1);
            }
            dates.Add(investmentDate);

            while (investmentDate.Date <= DateTime.Now.Date)
            {
                if (investmentDate.Day == 1)
                {
                    investmentDate = new DateTime(investmentDate.Year, investmentDate.Month, 15);
                }
                else
                {
                    investmentDate = new DateTime(investmentDate.Year, investmentDate.Month, 1).AddMonths(1);
                }
                if (investmentDate.Date <= DateTime.Now.Date)
                {
                    dates.Add(investmentDate);
                }                
            }

            return dates;
        }



        private Dictionary<string, decimal> CalculateHypotheticalPurchaseQuantity(decimal perDayInvestment, 
            IEnumerable<string> stockTickers, 
            IEnumerable<DateTime> dates,
            IEnumerable<StockIndexValue> stockIndexValues)
        {
            var dateTickerPriceMap = _stockIndexValueService.OrderIndexValuesByDateAndTicker(stockIndexValues);
            var unitCount = new Dictionary<string, decimal>();
            foreach (var ticker in stockTickers)
            {
                unitCount[ticker] = 0.00m;
            }

            foreach (var ticker in stockTickers)
            {
                foreach (var date in dates)
                {
                    var price = GetCurrentPrice(dateTickerPriceMap, date.Date, ticker);
                    if (price == 0.00m)
                        price = GetPreviousPrice(dateTickerPriceMap, date.Date, ticker);
                    if (price == 0.00m)
                        price = GetNextPrice(dateTickerPriceMap, date.Date, ticker);
                    if (price != 0.00m)
                        unitCount[ticker] += (perDayInvestment / price);
                }
            }

            return unitCount;
        }

        private decimal GetCurrentPrice(Dictionary<DateTime, Dictionary<string, decimal>> dateTickerPriceMap, DateTime date, string ticker)
        {
            if (dateTickerPriceMap.ContainsKey(date))
            {
                var tickerValues = dateTickerPriceMap[date];
                if (tickerValues.ContainsKey(ticker))
                {
                    var value = tickerValues[ticker];
                    return value;
                }
            }

            return 0.00m;
        }

        private decimal GetPreviousPrice(Dictionary<DateTime, Dictionary<string, decimal>> dateTickerPriceMap, DateTime date, string ticker)
        {
            var counter = 0;
            while (counter++ < 5)
            {
                date = date.AddDays(-1);
                if (dateTickerPriceMap.ContainsKey(date))
                {
                    var tickerValues = dateTickerPriceMap[date];
                    if (tickerValues.ContainsKey(ticker))
                    {
                        var value = tickerValues[ticker];
                        return value;
                    }
                }
            }

            return 0.00m;          
        }

        private decimal GetNextPrice(Dictionary<DateTime, Dictionary<string, decimal>> dateTickerPriceMap, DateTime date, string ticker)
        {
            var counter = 0;
            while (counter++ < 5)
            {
                date = date.AddDays(1);
                if (dateTickerPriceMap.ContainsKey(date))
                {
                    var tickerValues = dateTickerPriceMap[date];
                    if (tickerValues.ContainsKey(ticker))
                    {
                        var value = tickerValues[ticker];
                        return value;
                    }
                }
            }

            return 0.00m;
        }
    }
}
