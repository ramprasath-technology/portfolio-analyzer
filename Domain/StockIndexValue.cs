using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class StockIndexValue
    {
        public ulong ValueId { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public decimal DayHighValue { get; set; }
        public decimal DayLowValue { get; set; }
        public int TickerId { get; set; }
        public StockIndexTicker StockIndexTicker { get; set; }
    }
}
