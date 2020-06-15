using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO.StockAnalysis
{
    public class StockComparisonToIndex
    {
        public string Ticker { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal PriceDifferencePerQuantity { get; set; }
        public decimal TotalPriceDifference { get; set; }
        public decimal TotalCurrentPrice { get; set; }
        public decimal PercentageChange { get; set; }
        public decimal TotalPurchasePrice { get; set; }
        public IList<IndexDifference> IndexesDifference { get; set; }
    }

    public class IndexDifference
    {
        public string IndexTicker { get; set; }
        public decimal PriceOnPurchaseDate { get; set; }
        public decimal PurchaseQuantity { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal PriceDifferencePerQuantity { get; set; }
        public decimal TotalPriceDifference { get; set; }
        public decimal PercentageChange { get; set; }
        public decimal TotalPurchasePrice { get; set; }
        public decimal TotalCurrentPrice { get; set; }
    }
}
