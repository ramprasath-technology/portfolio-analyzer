using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO.StockAnalysis
{
    public class IndividualStockReturn
    {
        public string Ticker { get; set; }
        public decimal TotalInvestedAmount { get; set; }
        public decimal TotalCurrentValue { get; set; }
        public decimal PercentageOfPortfolio { get; set; }
        public decimal PercentageOfInvestedAmount { get; set; }
        public decimal TotalReturnPercentage { get; set; }
        public decimal DifferenceFromMedianPriceTargetPercentage { get; set; }
        public decimal DifferenceFromFifyTwoWeekHighPercentage { get; set; }
        public decimal DifferenceFromBiggestIndexGain { get; set; }
        public Dictionary<string, IndexReturn> IndexReturns { get; set; }
    }

    public class IndexReturn
    {
        public string IndexTicker { get; set; }
        public decimal TotalCurrentValue { get; set; }
        public decimal TotalReturnPercentage { get; set; }
    }
}
