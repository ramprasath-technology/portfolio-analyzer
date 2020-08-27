using Domain.DTO.WebScraper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO.StockAnalysis
{
    public class CompositionAndRecommendation
    {
        public long UserId { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalInvestmentValue { get; set; }
        public Dictionary<string, Tuple<IndividualStockWeightage, ScraperAnalysis>> Analyses { get; set; }
    }
}
