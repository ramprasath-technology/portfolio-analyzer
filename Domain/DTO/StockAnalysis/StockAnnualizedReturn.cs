using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO.StockAnalysis
{
    public class StockAnnualizedReturn
    {
        public string Ticker { get; set; }
        public decimal Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal CurrentPrice { get; set; }
        public DateTime CurrentDate { get; set; }
        public decimal TotalReturn { get; set; }
        public double AnnualizedReturn { get; set; }
        public string AnnualizedReturnDescription { get; set; }
        public string TotalReturnDescription { get; set; }

    }
}
