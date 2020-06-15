using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO.StockAnalysis
{
    public class TotalValueComparisonToIndex
    {
        public decimal TotalPurchasePrice { get; set; }
        public decimal TotalCurrentPrice { get; set; }
        public decimal TotalGainOrLoss { get; set; }
        public decimal ReturnPercentage { get; set; }
        public Dictionary<string, decimal> IndexGainOrLossMap { get; set; }
        public Dictionary<string, decimal> IndexTotalPurchasePriceMap { get; set; }
        public Dictionary<string, decimal> IndexTotalCurrentPriceMap { get; set; }
        public Dictionary<string, decimal> IndexTotalReturnPercentageMap { get; set; }
    }
}
