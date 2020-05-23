using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Stock
    {
        public ulong StockId { get; set; }
        public string Ticker { get; set; }
        public string CompanyName { get; set; }
    }
}
