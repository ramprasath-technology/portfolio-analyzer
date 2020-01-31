using Application.HoldingsService;
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
        private IHoldingsService _holdingsService;

        public StockAnalyzerService(IHoldingsService holdingsService)
        {
            _holdingsService = holdingsService;
        }

        public async void GetComparisonWithSP500(ulong userId)
        {
            var userHoldings = _holdingsService.GetHoldingsForUser(userId);
            var purchaseAndSaleDates = await GetPurchaseAndSaleDates(userHoldings);
            var datePriceMappingTask = GetSP500ValuesForDates(purchaseAndSaleDates);
            var stockTransactionDataTask = BuildStockTransactionData(userHoldings);
            var datePriceMapping = await datePriceMappingTask;
            var stockTransactionData = await stockTransactionDataTask;
            var transactionDataWithPrices = await AddSP500DataToTransactionData(stockTransactionData, datePriceMapping);
        }

        private async void ComputeReturnsForUserHoldings(HoldingsSP500Mapping[] holdingsSP500Mapping)
        {
            Parallel.ForEach(holdingsSP500Mapping, (currentMapping, state, index) =>
            {

            });
        }

        private decimal ComputeAnnualizedReturn(decimal purchasePrice, decimal salePrice, DateTime purchaseDate, DateTime saleDateOrCurrentDate)
        {
            const short numberOfDaysInYear = 365;

            var numberOfDaysStockHeld = (saleDateOrCurrentDate - purchaseDate).TotalDays;
            var daysRatio = numberOfDaysInYear / numberOfDaysStockHeld;
            var cumulativeReturn = (salePrice - purchasePrice) / purchasePrice;
            var annualizedReturn = Math.Pow(1 + Decimal.ToDouble(cumulativeReturn), daysRatio) - 1;

            return Convert.ToDecimal(annualizedReturn);
        }

        private async Task<List<DateTime>> GetPurchaseAndSaleDates(List<Holdings> userHoldings)
        {
            var purchaseAndSaleDates = new HashSet<DateTime>();

            Parallel.ForEach(userHoldings, (currentHolding) =>
           {
               purchaseAndSaleDates.Add(currentHolding.Purchase.Date);
               purchaseAndSaleDates.Add(currentHolding.Sale.Date);
           });

            return purchaseAndSaleDates.AsParallel().ToList();
        }

        private async Task<HoldingsSP500Mapping[]> AddSP500DataToTransactionData(HoldingsSP500Mapping[] holdingsSP500Mappings, Dictionary<DateTime, decimal> datePriceMapping)
        {
            Parallel.ForEach(holdingsSP500Mappings, (currentMapping, state, index) => 
            {
                decimal price;
                if(datePriceMapping.TryGetValue(currentMapping.PurchaseDate, out price))
                {
                    currentMapping.PurchaseDaySP500Value = price;
                }
                if(datePriceMapping.TryGetValue(currentMapping.SaleDate, out price))
                {
                    currentMapping.SaleDaySP500Value = price;
                }               
            });

            return holdingsSP500Mappings;
        }

        private async Task<HoldingsSP500Mapping[]> BuildStockTransactionData(List<Holdings> userHoldings)
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
