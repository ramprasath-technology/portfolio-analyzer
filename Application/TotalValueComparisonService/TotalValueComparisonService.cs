using Application.StockIndexComparisonService;
using Domain.DTO.StockAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.TotalValueComparisonService
{
    public class TotalValueComparisonService : ITotalValueComparisonService
    {
        private readonly IStockIndexComparisonService _stockIndexComparisonService;
        public TotalValueComparisonService(IStockIndexComparisonService stockIndexComparisonService)
        {
            _stockIndexComparisonService = stockIndexComparisonService;
        }

        public async Task<TotalValueComparisonToIndex> GetTotalValueComparison(ulong userId, IEnumerable<string> indexTickers)
        {
            var valueComparisons = await _stockIndexComparisonService.GetComparisonWithIndex(userId, indexTickers);
            var totalValueComparison = new TotalValueComparisonToIndex();
            totalValueComparison.IndexGainOrLossMap = new Dictionary<string, decimal>();
            totalValueComparison.IndexTotalCurrentPriceMap = new Dictionary<string, decimal>();
            totalValueComparison.IndexTotalPurchasePriceMap = new Dictionary<string, decimal>();
            totalValueComparison.IndexTotalReturnPercentageMap = new Dictionary<string, decimal>();

            foreach (var comparison in valueComparisons)
            {
                totalValueComparison.TotalGainOrLoss += comparison.TotalPriceDifference;
                totalValueComparison.TotalCurrentPrice += comparison.TotalCurrentPrice;
                totalValueComparison.TotalPurchasePrice += comparison.TotalPurchasePrice;

                foreach (var indexDifference in comparison.IndexesDifference)
                {
                    if (!totalValueComparison.IndexGainOrLossMap.ContainsKey(indexDifference.IndexTicker))
                    {
                        totalValueComparison.IndexGainOrLossMap[indexDifference.IndexTicker] = indexDifference.TotalPriceDifference;
                        totalValueComparison.IndexTotalCurrentPriceMap[indexDifference.IndexTicker] = indexDifference.TotalCurrentPrice;
                        totalValueComparison.IndexTotalPurchasePriceMap[indexDifference.IndexTicker] = indexDifference.TotalPurchasePrice;
                    }
                    else
                    {
                        totalValueComparison.IndexGainOrLossMap[indexDifference.IndexTicker] += indexDifference.TotalPriceDifference;
                        totalValueComparison.IndexTotalCurrentPriceMap[indexDifference.IndexTicker] += indexDifference.TotalCurrentPrice;
                        totalValueComparison.IndexTotalPurchasePriceMap[indexDifference.IndexTicker] += indexDifference.TotalPurchasePrice;
                    }
                }
            }

            foreach (var indexTicker in totalValueComparison.IndexTotalPurchasePriceMap.Keys)
            {
                if(totalValueComparison.IndexTotalCurrentPriceMap.ContainsKey(indexTicker))
                {
                    totalValueComparison.IndexTotalReturnPercentageMap[indexTicker] = ((totalValueComparison.IndexTotalCurrentPriceMap[indexTicker] - totalValueComparison.IndexTotalPurchasePriceMap[indexTicker]) / totalValueComparison.IndexTotalPurchasePriceMap[indexTicker]) * 100;
                }
            }

            totalValueComparison.ReturnPercentage = ((totalValueComparison.TotalCurrentPrice - totalValueComparison.TotalPurchasePrice) / totalValueComparison.TotalPurchasePrice) * 100;

            return totalValueComparison;
        }
    }
}
