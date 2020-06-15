using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO.StockAnalysis
{
    public class TotalHoldingsValue
    {
        public ulong UserId { get; set; }
        public decimal  TotalHoldingPurchasePrice { get; set; }
        public decimal TotalHoldingCurrentPrice { get; set; }
        public decimal TotalHoldingGainOrLoss { get; set; }
        public decimal TotalHoldingGainOrLossPercentage { get; set; }
        public IEnumerable<IndividualStockValue> IndividualStockValues { get; set; }
    }

    public class IndividualStockValue
    {
        public string Ticker { get; set; }
        public decimal TotalStockPurchasePrice { get; set; }
        public decimal TotalStockCurrentPrice { get; set; }
        public decimal TotalStockGainOrLoss { get; set; }
        public decimal TotalStockGainOrLossPercentage { get; set; }
    }
}
