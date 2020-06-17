using Dapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.StockIndexValueData
{
    public class StockIndexValueData : IStockIndexValueData
    {
        #region Queries
        private readonly string selLastDateForTicker =
            @"SELECT s.date AS Date,
                 s.day_high_value AS DayHighValue,
                 s.day_low_value AS DayLowValue,
                 s.value AS Value,
                 s.ticker_id AS TickerId
            FROM stock_index_value s
                 INNER JOIN stock_index_ticker s1 ON s.ticker_id = s1.ticker_id
            WHERE s1.ticker = ?ticker
            ORDER BY s.date DESC
            LIMIT 1;";

        private readonly string insDailyTickerValue =
            @"INSERT INTO stock_index_value(ticker_id,
                              `date`,
                              value,
                              day_high_value,
                              day_low_value)
             VALUES (@TickerId,
                     @Date,
                     @Value,
                     @DayHighValue,
                     @DayLowValue);";

        private readonly string selNumberOfDaysTickerValueIsPresent =
            @"SELECT COUNT(*)
                     FROM stock_index_value s
                    WHERE s.ticker_id = ?tickerId AND s.`date` BETWEEN ?startDate AND ?endDate;";

        private readonly string selPricesForGivenIndexAndDate =
            @"SELECT s.`date` AS Date,
                   s.value AS Value,
                   s.value_id AS ValueId,
                   s.day_high_value AS DayHighValue,
                   s.day_low_value AS DayLowValue,
                   s1.ticker_id AS TickerId,
                   s1.ticker AS Ticker,
                   s1.ticker_description AS TickerDescription
              FROM stock_index_value s
              INNER JOIN stock_index_ticker s1
              ON s.ticker_id = s1.ticker_id
              WHERE s1.ticker IN ?ticker
              AND date(s.`date`) IN ?date;";
        #endregion

        public async Task<StockIndexValue> GetLastValueForTicker(IDbConnection connection, string ticker)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ticker", ticker);

            connection.Open();
            var lastIndexValue = await connection.QueryFirstAsync<StockIndexValue>(selLastDateForTicker, parameters);
            connection.Close();

            return lastIndexValue;
        }

        public async Task AddIndexValue(IDbConnection connection, IEnumerable<StockIndexValue> stockIndexValue)
        {
            connection.Open();
            await connection.ExecuteAsync(insDailyTickerValue, stockIndexValue);
            connection.Close();
        }

        public async Task<int> GetNumberOfDaysTickerValueIsPresent(IDbConnection connection, int tickerId, DateTime startDate, DateTime endDate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("tickerId", tickerId);
            parameters.Add("startDate", startDate);
            parameters.Add("endDate", endDate);

            connection.Open();
            var numberOfDays = await connection.ExecuteScalarAsync<int>(selNumberOfDaysTickerValueIsPresent, parameters);
            connection.Close();

            return numberOfDays;
        }

        public async Task<IEnumerable<StockIndexValue>> GetPricesForGivenIndexAndDate(IDbConnection connection,
            IEnumerable<string> ticker,
            IEnumerable<DateTime> date)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ticker", ticker);
            parameters.Add("date", date);

            var values = await connection.QueryAsync<StockIndexValue, StockIndexTicker, StockIndexValue>(
                selPricesForGivenIndexAndDate,
                map: (indexValue, indexTicker) =>
                {
                    indexValue.StockIndexTicker = indexTicker;
                    return indexValue;
                },
                param: new { ticker = ticker, date = date},
                splitOn: "TickerId");

            return values;
        }
    }
}
