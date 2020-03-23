using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class HoldingsSP500Mapping
    {
        public ulong UserId { get; set; }
        public string Ticker { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime? SaleDate { get; set; }
        public decimal PurchaseValue { get; set; }
        public decimal? SaleValue { get; set; }
        public decimal PurchaseDaySP500Value { get; set; }
        public decimal? SaleDaySP500Value { get; set; }
        public decimal AnnualizedStockReturn { get; set; }
        public decimal AnnualizedSP500Return { get; set; }
        public decimal? CurrentStockValue { get; set; }
        public decimal? CurrentSP500Value { get; set; }

    }
}
