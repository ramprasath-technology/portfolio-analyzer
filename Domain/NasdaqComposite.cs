using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class NasdaqComposite
    {
        public int NasdaqCompositeId { get; set; }
        public string Ticker { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
    }
}
