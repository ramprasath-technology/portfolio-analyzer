using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class SP500
    {
        public uint SPId { get; set; }
        public string Ticker { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
    }
}
