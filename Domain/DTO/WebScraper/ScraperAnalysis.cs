using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO.WebScraper
{
    public class ScraperAnalysis
    {
        public string Ticker { get; set; }
        public decimal FiftyTwoWeekLow { get; set; }
        public decimal FiftyTwoWeekHigh { get; set; }
        public int NumberOfAnalysts { get; set; }
        public decimal LowTarget { get; set; }
        public decimal HighTarget { get; set; }
        public decimal MedianTarget { get; set; }
        public decimal DifferenceFromMedianPercentage { get; set; }
        public decimal DecreaseFromFiftyTwoWeekHigh { get; set; }
        public decimal DecreaseFromFiftyTwoWeekHighPercentage { get; set; }
        public decimal IncreaseFromFiftyTwoWeekLow { get; set; }
        public decimal IncreaseFromFiftyTwoWeekLowPercentage { get; set; }
        public decimal CurrentPrice { get; set; }
        public string CurrentRecommendation { get; set; }

    }
}
