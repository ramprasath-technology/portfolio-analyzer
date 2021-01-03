using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO.StockAnalysis
{
    public class LifeTimeComparison
    {
        private decimal totalRealizedGainOrLoss;
        private decimal totalPurchase;
        private decimal totalSale;
        private decimal totalPaperGainOrLoss;
        private decimal totalGainOrLoss;
        private decimal totalReturnPercentage;
        public decimal TotalPurchase 
        {
            get { return totalPurchase; } 
            set { totalPurchase = Decimal.Round(value, 2, MidpointRounding.AwayFromZero); } 
        }
        public decimal TotalSale 
        { 
            get { return totalSale; } 
            set { totalSale = Decimal.Round(value, 2, MidpointRounding.AwayFromZero); } 
        }
        public decimal TotalRealizedGainOrLoss 
        { 
            get { return totalRealizedGainOrLoss; } 
            set { totalRealizedGainOrLoss = Decimal.Round(value, 2); } 
        }
        public decimal TotalPaperGainOrLoss 
        { 
            get { return totalPaperGainOrLoss; } 
            set { totalPaperGainOrLoss = Decimal.Round(value, 2, MidpointRounding.AwayFromZero); } 
        }
        public decimal TotalGainOrLoss 
        { 
            get { return totalGainOrLoss; } 
            set { totalGainOrLoss = Decimal.Round(value, 2, MidpointRounding.AwayFromZero); } 
        }
        public decimal TotalReturnPercentage 
        { 
            get { return totalReturnPercentage; } 
            set { totalReturnPercentage = Decimal.Round(value, 2, MidpointRounding.AwayFromZero); } 
        }
        public Dictionary<string, IndexLifeTimeComparison> IndexLifeTimeComparisonMap { get; set; }
    }

    public class IndexLifeTimeComparison
    {
        private decimal totalPurchaseUnits;
        private decimal totalSaleUnits;
        private decimal totalRealizedGainOrLoss;
        private decimal totalPaperGainOrLoss;
        private decimal totalGainOrLoss;
        private decimal totalReturnPercentage;

        public decimal TotalPurchaseUnits
        { 
            get { return totalPurchaseUnits; }
            set { totalPurchaseUnits = Decimal.Round(value, 2, MidpointRounding.AwayFromZero); }
        }
        public decimal TotalSaleUnits 
        { 
            get { return totalSaleUnits; } 
            set { totalSaleUnits = Decimal.Round(value, 2, MidpointRounding.AwayFromZero); } 
        }
        public decimal TotalRealizedGainOrLoss 
        { 
            get { return totalRealizedGainOrLoss; }
            set { totalRealizedGainOrLoss = Decimal.Round(value, 2, MidpointRounding.AwayFromZero); }
        }
        public decimal TotalPaperGainOrLoss 
        { 
            get { return totalPaperGainOrLoss; } 
            set { totalPaperGainOrLoss = Decimal.Round(value, 2, MidpointRounding.AwayFromZero); }
        }
        public decimal TotalGainOrLoss 
        { 
            get { return totalGainOrLoss; } 
            set { totalGainOrLoss = Decimal.Round(value, 2, MidpointRounding.AwayFromZero); } 
        }
        public decimal TotalReturnPercentage
        {
            get { return totalReturnPercentage; }
            set { totalReturnPercentage = Decimal.Round(value, 2, MidpointRounding.AwayFromZero); }
        }
    }
}
