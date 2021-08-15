using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TransactionHistory.Fidelity
{
    public class FidelityTransactionColumns
    {
        public int RunDate { get; set; }
        public int Action { get; set; }
        public int Symbol { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
