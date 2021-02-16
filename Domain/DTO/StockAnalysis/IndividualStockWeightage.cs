using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO.StockAnalysis
{
    public class IndividualStockWeightage
    {
        public string Ticker { get; set; }
        public decimal TotalPurchasePrice { get; set; }
        public decimal TotalCurrentValue { get; set; }
        public decimal PurchasePriceRatio { get; set; }
        public decimal CurrentValueRatio { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }
        public string Country { get; set; }
    }
}
