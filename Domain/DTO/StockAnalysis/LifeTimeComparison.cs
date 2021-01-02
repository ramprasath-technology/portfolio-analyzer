using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO.StockAnalysis
{
    public class LifeTimeComparison
    {
        public decimal TotalPurchase { get; set; }
        public decimal TotalSale { get; set; }
        public decimal TotalRealizedGainOrLoss { get; set; }
        public decimal TotalPaperGainOrLoss { get; set; }
    }
}
