using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class StockSplit
    {
        public string Ticker { get; set; }
        public decimal OldStockRatio { get; set; }
        public decimal NewStockRatio { get; set; }
        public DateTime EffectiveDate { get; set; }
        public Nullable<DateTime> LastSplitDate { get; set; }
    }
}
