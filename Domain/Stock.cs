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
        public string Industry { get; set; }
        public string Sector { get; set; }
        public string Country { get; set; }

    }
}
