using Application.StockIndexComparisonService;
using Domain.DTO.StockAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<TotalValueComparisonToIndex> GetTotalValueComparison(ulong userId, 
            IEnumerable<string> indexTickers, 
            DateTime from, 
            DateTime to)
        {
            var valueComparisons = await _stockIndexComparisonService.GetComparisonWithIndex(userId, indexTickers, from, to);
            var totalValueComparison = new TotalValueComparisonToIndex();
            totalValueComparison.IndexGainOrLossMap = new Dictionary<string, decimal>();
            totalValueComparison.IndexTotalCurrentPriceMap = new Dictionary<string, decimal>();
            totalValueComparison.IndexTotalPurchasePriceMap = new Dictionary<string, decimal>();
            totalValueComparison.IndexTotalReturnPercentageMap = new Dictionary<string, decimal>();
            foreach (var comparison in valueComparisons)
            {
                totalValueComparison.TotalGainOrLoss += decimal.Round(comparison.TotalPriceDifference, 2, MidpointRounding.AwayFromZero);
                totalValueComparison.TotalCurrentPrice += decimal.Round(comparison.TotalCurrentPrice, 2, MidpointRounding.AwayFromZero);
                totalValueComparison.TotalPurchasePrice += decimal.Round(comparison.TotalPurchasePrice, 2, MidpointRounding.AwayFromZero);

                foreach (var indexDifference in comparison.IndexesDifference)
                {
                    if (!totalValueComparison.IndexGainOrLossMap.ContainsKey(indexDifference.IndexTicker))
                    {
                        totalValueComparison.IndexGainOrLossMap[indexDifference.IndexTicker] = decimal.Round(indexDifference.TotalPriceDifference, 2, MidpointRounding.AwayFromZero);
                        totalValueComparison.IndexTotalCurrentPriceMap[indexDifference.IndexTicker] = decimal.Round(indexDifference.TotalCurrentPrice, 2, MidpointRounding.AwayFromZero);
                        totalValueComparison.IndexTotalPurchasePriceMap[indexDifference.IndexTicker] = decimal.Round(indexDifference.TotalPurchasePrice, 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        totalValueComparison.IndexGainOrLossMap[indexDifference.IndexTicker] += decimal.Round(indexDifference.TotalPriceDifference, 2, MidpointRounding.AwayFromZero);
                        totalValueComparison.IndexTotalCurrentPriceMap[indexDifference.IndexTicker] += decimal.Round(indexDifference.TotalCurrentPrice, 2, MidpointRounding.AwayFromZero);
                        totalValueComparison.IndexTotalPurchasePriceMap[indexDifference.IndexTicker] += decimal.Round(indexDifference.TotalPurchasePrice, 2, MidpointRounding.AwayFromZero);
                    }
                }
            }

            foreach (var indexTicker in totalValueComparison.IndexTotalPurchasePriceMap.Keys)
            {
                if(totalValueComparison.IndexTotalCurrentPriceMap.ContainsKey(indexTicker))
                {
                    totalValueComparison.IndexTotalReturnPercentageMap[indexTicker] = decimal.Round(((totalValueComparison.IndexTotalCurrentPriceMap[indexTicker] - totalValueComparison.IndexTotalPurchasePriceMap[indexTicker]) / totalValueComparison.IndexTotalPurchasePriceMap[indexTicker]) * 100, 2, MidpointRounding.AwayFromZero);
                }
            }

            totalValueComparison.ReturnPercentage = decimal.Round(((totalValueComparison.TotalCurrentPrice - totalValueComparison.TotalPurchasePrice) / totalValueComparison.TotalPurchasePrice) * 100, 2, MidpointRounding.AwayFromZero);

            return totalValueComparison;
        }   
    }
}
