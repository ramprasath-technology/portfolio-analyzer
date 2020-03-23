using Application.HoldingsService;
using Application.StockQuoteService;
using Domain;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockAnalyzerService
{
    public class StockAnalyzerService
    {
        private readonly IHoldingsService _holdingsService;
        private readonly IStockQuoteService _stockQuoteService;

        public StockAnalyzerService(IHoldingsService holdingsService, IStockQuoteService stockQuoteService)
        {
            _holdingsService = holdingsService;
            _stockQuoteService = stockQuoteService;
        }

        public async void GetComparisonWithSP500(ulong userId)
        {
            var userHoldings = _holdingsService.GetHoldingsForUser(userId);
            var purchaseAndSaleDates = GetPurchaseAndSaleDates(userHoldings);
            var datePriceMappingTask = GetSP500ValuesForDates(purchaseAndSaleDates);
            var stockTransactionData = BuildStockTransactionData(userHoldings);
            var datePriceMapping = await datePriceMappingTask;
            AddSP500DataToTransactionData(stockTransactionData, datePriceMapping);
            AssignCurrentStockValue(stockTransactionData);
            var annualizedReturnComputationTask = Task.Run(() => ComputeAnnualizedReturn(stockTransactionData));
            var annualizedSP500ReturnComputationTask = Task.Run(() => ComputeAnnualizedSP500Return(stockTransactionData));
            await annualizedReturnComputationTask;
            await annualizedSP500ReturnComputationTask;
        }

        private void AssignCurrentStockValue(HoldingsSP500Mapping[] holdingsSP500Mapping)
        {
            var stocksStillHeld = new HashSet<string>();

            Parallel.ForEach(holdingsSP500Mapping, (currenHolding) =>
            {
                if(!currenHolding.SaleDate.HasValue)
                {
                    stocksStillHeld.Add(currenHolding.Ticker);
                }
            });

            var latestQuotes = _stockQuoteService.GetLatestQuoteForStocks(stocksStillHeld).Result;

            Parallel.ForEach(holdingsSP500Mapping, (currentHolding) =>
            {
                if(latestQuotes.ContainsKey(currentHolding.Ticker))
                {
                    currentHolding.CurrentStockValue = latestQuotes[currentHolding.Ticker];
                }
                else
                {
                    currentHolding.CurrentStockValue = 0;
                }
            });
        }

        private void ComputeAnnualizedReturn(HoldingsSP500Mapping[] holdingsSP500Mapping)
        {
            const short numberOfDaysInYear = 365;

            for(var i = 0; i < holdingsSP500Mapping.Length; i++)
            {
                var saleDateOrCurrentDate = holdingsSP500Mapping[i].SaleDate ?? DateTime.Now;
                var numberOfDaysStockHeld = (saleDateOrCurrentDate - holdingsSP500Mapping[i].PurchaseDate).TotalDays;
                var daysRatio = numberOfDaysInYear / numberOfDaysStockHeld;
                var salePriceOrCurrentPrice = holdingsSP500Mapping[i].SaleValue ?? holdingsSP500Mapping[i].CurrentStockValue;
                var cumulativeReturn = (salePriceOrCurrentPrice.Value - holdingsSP500Mapping[i].PurchaseValue) / holdingsSP500Mapping[i].PurchaseValue;
                var annualizedReturn = Math.Pow(1 + Decimal.ToDouble(cumulativeReturn), daysRatio) - 1;
                holdingsSP500Mapping[i].AnnualizedStockReturn = Convert.ToDecimal(annualizedReturn);
            }
        }

        private void ComputeAnnualizedSP500Return(HoldingsSP500Mapping[] holdingsSP500Mapping)
        {
            const short numberOfDaysInYear = 365;

            for (var i = 0; i < holdingsSP500Mapping.Length; i++)
            {
                var saleDateOrCurrentDate = holdingsSP500Mapping[i].SaleDate ?? DateTime.Now;
                var numberOfDaysStockHeld = (saleDateOrCurrentDate - holdingsSP500Mapping[i].PurchaseDate).TotalDays;
                var daysRatio = numberOfDaysInYear / numberOfDaysStockHeld;
                var salePriceOrCurrentPrice = holdingsSP500Mapping[i].SaleDaySP500Value ?? holdingsSP500Mapping[i].CurrentSP500Value;
                var cumulativeReturn = (salePriceOrCurrentPrice.Value - holdingsSP500Mapping[i].PurchaseValue) / holdingsSP500Mapping[i].PurchaseValue;
                var annualizedReturn = Math.Pow(1 + Decimal.ToDouble(cumulativeReturn), daysRatio) - 1;
                holdingsSP500Mapping[i].AnnualizedSP500Return = Convert.ToDecimal(annualizedReturn);
            }
        }

        private List<DateTime> GetPurchaseAndSaleDates(List<Holdings> userHoldings)
        {
            var purchaseAndSaleDates = new HashSet<DateTime>();

            Parallel.ForEach(userHoldings, (currentHolding) =>
           {
               purchaseAndSaleDates.Add(currentHolding.Purchase.Date);
               purchaseAndSaleDates.Add(currentHolding.Sale.Date);
           });

            return purchaseAndSaleDates.AsParallel().ToList();
        }

        private void AddSP500DataToTransactionData(HoldingsSP500Mapping[] holdingsSP500Mappings, Dictionary<DateTime, decimal> datePriceMapping)
        {
            Parallel.ForEach(holdingsSP500Mappings, (currentMapping, state, index) => 
            {
                decimal price;
                if(datePriceMapping.TryGetValue(currentMapping.PurchaseDate, out price))
                {
                    currentMapping.PurchaseDaySP500Value = price;
                }
             
                if (currentMapping.SaleDate.HasValue && datePriceMapping.TryGetValue(currentMapping.SaleDate.Value, out price))
                {
                    currentMapping.SaleDaySP500Value = price;
                }
                
            });
        }

        private HoldingsSP500Mapping[] BuildStockTransactionData(List<Holdings> userHoldings)
        {
            var holdingsSP500Mappings = new HoldingsSP500Mapping[userHoldings.Count];

            Parallel.ForEach(userHoldings, (currentHolding, state, index) =>
            {
                holdingsSP500Mappings[index] = new HoldingsSP500Mapping()
                {
                    PurchaseDate = currentHolding.Purchase.Date,
                    PurchaseValue = currentHolding.Purchase.Price,
                    SaleDate = currentHolding.Sale.Date,
                    SaleValue = currentHolding.Sale.Price,
                    Ticker = currentHolding.Stock.Ticker,
                    UserId = currentHolding.UserId
                };
            });

            return holdingsSP500Mappings;
        }

        private async Task<Dictionary<DateTime, decimal>> GetSP500ValuesForDates(List<DateTime> purchaseAndSaleDates)
        {
            return new Dictionary<DateTime, decimal>();
        }
    }
}
