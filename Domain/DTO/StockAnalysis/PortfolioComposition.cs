using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.DTO.StockAnalysis
{
    public class PortfolioComposition
    {
        public decimal TotalCost { get; set; }
        public decimal TotalInvestmentValue { get; set; }
        public IEnumerable<IndividualStockWeightage> IndividualStockWeightages { get; set; }
        [JsonIgnore]
        public Dictionary<string, decimal> InvestmentAmountByCountry { get; set; }
        [JsonIgnore]
        public Dictionary<string, decimal> InvestmentAmountBySector { get; set; }
        [JsonIgnore]
        public Dictionary<string, decimal> InvestmentAmountByIndustry { get; set; }
        [JsonIgnore]
        public Dictionary<string, decimal> CurrentValueAmountByCountry { get; set; }
        [JsonIgnore]
        public Dictionary<string, decimal> CurrentValueAmountBySector { get; set; }
        [JsonIgnore]
        public Dictionary<string, decimal> CurrentValueAmountByIndustry { get; set; }
        public Dictionary<string, decimal> InvestmentRatioByCountry { get; set; }
        public Dictionary<string, decimal> InvestmentRatioBySector { get; set; }
        public Dictionary<string, decimal> InvestmentRatioByIndustry { get; set; }
        public Dictionary<string, decimal> CurrentValueRatioByCountry { get; set; }
        public Dictionary<string, decimal> CurrentValueRatioBySector { get; set; }
        public Dictionary<string, decimal> CurrentValueRatioByIndustry { get; set; }

        public PortfolioComposition()
        {
            CurrentValueAmountByCountry = new Dictionary<string, decimal>();
            CurrentValueAmountByIndustry = new Dictionary<string, decimal>();
            CurrentValueAmountBySector = new Dictionary<string, decimal>();
            CurrentValueRatioByCountry = new Dictionary<string, decimal>();
            CurrentValueRatioByIndustry = new Dictionary<string, decimal>();
            CurrentValueRatioBySector = new Dictionary<string, decimal>();
            InvestmentAmountByCountry = new Dictionary<string, decimal>();
            InvestmentAmountByIndustry = new Dictionary<string, decimal>();
            InvestmentAmountBySector = new Dictionary<string, decimal>();
            InvestmentRatioByCountry = new Dictionary<string, decimal>();
            InvestmentRatioByIndustry = new Dictionary<string, decimal>();
            InvestmentRatioBySector = new Dictionary<string, decimal>();
        }
    }
}
