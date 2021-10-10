using Application.CompositionAndRecommendationService;
using Application.ExtensionMethods;
using Application.StockIndexComparisonService;
using Domain.DTO;
using Domain.DTO.StockAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IndividualStockComparisonService
{
    public class IndividualStockComparisonService : IIndividualStockComparisonService
    {
        private readonly IStockIndexComparisonService _stockIndexComparisonService;
        private readonly ICompositionAndRecommendationService _compositionAndRecommendationService;

        public IndividualStockComparisonService(IStockIndexComparisonService stockIndexComparisonService, 
            ICompositionAndRecommendationService compositionAndRecommendationService)
        {
            _stockIndexComparisonService = stockIndexComparisonService;
            _compositionAndRecommendationService = compositionAndRecommendationService;
        }

        public async Task<IEnumerable<IndividualStockReturn>> GetIndividualComparisonService(ulong userId)
        {
            var indexTickers = IndexTickers.GetAllowedIndexTickers();
            var comparisons = await _stockIndexComparisonService.GetComparisonWithIndex(userId, indexTickers);
            var recommendationTask = _compositionAndRecommendationService.GetAnalystRecommendation(comparisons.Select(x => x.Ticker));
            var stockReturn = new Dictionary<string, IndividualStockReturn>();
            var totalPorfolioValue = 0.0m;
            var totalInvestment = 0.0m;
            var individualReturns = new List<IndividualStockReturn>();
            
            foreach (var comparison in comparisons)
            {
                totalPorfolioValue += comparison.TotalCurrentPrice;
                totalInvestment += comparison.TotalPurchasePrice;
                if (stockReturn.ContainsKey(comparison.Ticker))
                {
                    var returnDetails = stockReturn[comparison.Ticker];
                    returnDetails.TotalInvestedAmount += comparison.TotalPurchasePrice;
                    returnDetails.TotalCurrentValue += comparison.TotalCurrentPrice;
                    foreach (var indexValue in comparison.IndexesDifference)
                    {
                        var indexComparison = returnDetails.IndexReturns[indexValue.IndexTicker];
                        indexComparison.TotalCurrentValue += indexValue.TotalCurrentPrice;
                    }
                }
                else
                {
                    var returnDetails = new IndividualStockReturn();
                    returnDetails.Ticker = comparison.Ticker;
                    returnDetails.TotalInvestedAmount = comparison.TotalPurchasePrice;
                    returnDetails.TotalCurrentValue = comparison.TotalCurrentPrice;
                    var indexReturns = new Dictionary<string, IndexReturn>();
                    foreach (var indexValue in comparison.IndexesDifference)
                    {
                        var indexReturn = new IndexReturn();
                        indexReturn.IndexTicker = indexValue.IndexTicker;
                        indexReturn.TotalCurrentValue = indexValue.TotalCurrentPrice;
                        indexReturns[indexValue.IndexTicker] = indexReturn;
                    }
                    returnDetails.IndexReturns = indexReturns;
                    stockReturn[comparison.Ticker] = returnDetails;
                }              
            }
            var recommendation = await recommendationTask;
            foreach (var entry in stockReturn.Values)
            {
                entry.TotalReturnPercentage = Decimal.Round(((entry.TotalCurrentValue - entry.TotalInvestedAmount) / entry.TotalInvestedAmount) * 100, 2);
                entry.PercentageOfPortfolio = Decimal.Round((entry.TotalCurrentValue / totalPorfolioValue) * 100, 2);
                entry.PercentageOfInvestedAmount = Decimal.Round((entry.TotalInvestedAmount / totalInvestment) * 100, 2);
                var maxIndexGain = GetBiggestIndexGain(entry.IndexReturns.Values);
                var maxIndexGainPercentage = GetBiggestIndexGainPercentage(maxIndexGain, entry.TotalInvestedAmount);
                entry.DifferenceFromBiggestIndexGain = entry.TotalReturnPercentage - maxIndexGainPercentage;
                if (recommendation.ContainsKey(entry.Ticker))
                {
                    entry.DifferenceFromMedianPriceTargetPercentage = recommendation[entry.Ticker].DifferenceFromMedianPercentage;
                    entry.DifferenceFromFifyTwoWeekHighPercentage = recommendation[entry.Ticker].DecreaseFromFiftyTwoWeekHighPercentage;
                }
                individualReturns.Add(entry);
            }

            return individualReturns.OrderByDescending(x => x.DifferenceFromBiggestIndexGain);          
        }

        private decimal GetBiggestIndexGain(IEnumerable<IndexReturn> indexReturns)
        {
            if (indexReturns.Count() == 0)
            {
                return 0.0m;
            }
            var indexGain = indexReturns.First().TotalCurrentValue;

            foreach (var indexReturn in indexReturns)
            {
                indexGain = Math.Max(indexGain, indexReturn.TotalCurrentValue);
            }

            return indexGain;
        }

        private decimal GetBiggestIndexGainPercentage(decimal indexGain, decimal totalAmountInvested)
        {
            var gainPercentage = ((indexGain - totalAmountInvested) / totalAmountInvested) * 100;
            return gainPercentage.RoundDecimal();
        }
    }
}
