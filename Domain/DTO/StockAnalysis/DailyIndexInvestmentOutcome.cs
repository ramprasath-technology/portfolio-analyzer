using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO.StockAnalysis
{
    public class DailyIndexInvestmentOutcome
    {
        public decimal TotalInvestment { get; set; }
        public int TotalNumberOfDays { get; set; }
        public decimal InvestmentPerDay { get; set; }
        public Dictionary<string, decimal> HypotheticalCurrentValues { get; set; }
        public Dictionary<string, decimal> HypotheticalReturnPercentage { get; set; }
    }
}
