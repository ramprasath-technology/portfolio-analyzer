using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO.StockAnalysis
{
    public class PortfolioComposition
    {
        public decimal TotalCost { get; set; }
        public decimal TotalInvestmentValue { get; set; }
        public IEnumerable<IndividualStockWeightage> IndividualStockWeightages { get; set; }
    }
}
