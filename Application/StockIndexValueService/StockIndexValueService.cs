using Application.Connection;
using Application.MarketDataService;
using Application.StockIndexTickerService;
using Domain;
using Domain.DTO;
using Persistence.StockIndexValueData;
using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepDTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockIndexValueService
{
    public class StockIndexValueService : IStockIndexValueService
    {
        private readonly IStockIndexValueData _stockIndexValueData;
        private readonly IConnectionService _connectionService;
        private readonly IStockIndexTickerService _stockIndexTickerService;
        private readonly IMarketDataService _marketDataService;

        public StockIndexValueService(IStockIndexValueData stockIndexValueData, 
            IStockIndexTickerService stockIndexTickerService,
            IConnectionService connectionService,
            IMarketDataService marketDataService)
        {
            _stockIndexValueData = stockIndexValueData;
            _stockIndexTickerService = stockIndexTickerService;
            _connectionService = connectionService;
            _marketDataService = marketDataService;
        }

        public async Task AddIndexValues(StockIndexValueInputs stockIndexValueInputs)
        {
            if(ValidateInputs(stockIndexValueInputs) && await _stockIndexTickerService.CheckIfStockTickerExists(stockIndexValueInputs.Ticker))
            {               
                var ticker = await _stockIndexTickerService.GetStockIndex(stockIndexValueInputs.Ticker);
                var tickerId = ticker.TickerId;
                var connection = _connectionService.GetConnectionToCommonShard();
                var startTime = stockIndexValueInputs.StartDate;
                var endTime = stockIndexValueInputs.EndDate;

                var totalNumberOfDays = CalculateNumberOfWeekDays(startTime, endTime);
                var numberOfDaysTickerIsPresent =  await _stockIndexValueData.GetNumberOfDaysTickerValueIsPresent(connection, tickerId, startTime, endTime);
                if (numberOfDaysTickerIsPresent != totalNumberOfDays)
                {
                    var stockPrice = await _marketDataService.GetDailyStockPrice(stockIndexValueInputs.Ticker,
                            stockIndexValueInputs.StartDate,
                            stockIndexValueInputs.EndDate);
                    var indexValues = MapMarketDataToIndexValueModel(stockPrice, tickerId);
                    await _stockIndexValueData.AddIndexValue(connection, indexValues);
                }

                _connectionService.DisposeConnection(connection);
            }
        }

        private double CalculateNumberOfWeekDays(DateTime startTime, DateTime endTime)
        {
            //TODO Need to include logic for stock market holidays too
            var numOfDays = 0;

            while (startTime.Date <= endTime.Date)
            {
                if(startTime.DayOfWeek != DayOfWeek.Saturday && startTime.DayOfWeek != DayOfWeek.Sunday)
                {
                    numOfDays++;
                }

                startTime = startTime.AddDays(1);
            }

            return numOfDays;
        }

        private IEnumerable<StockIndexValue> MapMarketDataToIndexValueModel(DailyStockPrice dailyStockPrice, int tickerId)
        {
            var historicalData = dailyStockPrice.Historical;
            var historicalIndexValue = new List<StockIndexValue>();

            foreach(var dailyData in historicalData)
            {
                historicalIndexValue.Add(new StockIndexValue {
                    Date = dailyData.Date,
                    DayHighValue = Decimal.Round(dailyData.High, 2),
                    DayLowValue = Decimal.Round(dailyData.Low, 2),
                    Value = Decimal.Round(dailyData.Close, 2),
                    TickerId = tickerId                  
                });
            }

            return historicalIndexValue;
        }

        private bool ValidateInputs(StockIndexValueInputs stockIndexValueInputs)
        {
            var startDate = stockIndexValueInputs.StartDate.Date;
            var endDate = stockIndexValueInputs.EndDate.Date;

            if (endDate < startDate)
                return false;
            if (endDate > DateTime.Now.Date || startDate > DateTime.Now.Date)
                return false;

            return true;
        }

        public async Task<IEnumerable<StockIndexValue>> GetPricesForGivenIndexAndDate(ulong userId, 
            IEnumerable<string> tickers, 
            IEnumerable<DateTime> dates)
        {
            using(var connection = _connectionService.GetOpenConnection(userId))
            {
                var datesWithoutTime = dates.Select(x => x.Date);
                var indexValues = await _stockIndexValueData.GetPricesForGivenIndexAndDate(connection, tickers, datesWithoutTime);
                return indexValues;
            }
            
        }
    }
}
