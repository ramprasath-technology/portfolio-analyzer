using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Holdings
    {
        public ulong HoldingId { get; set; }
        public ulong StockId { get; set; }
        public ulong UserId { get; set; }
        public IEnumerable<HoldingDetails> HoldingDetails { get; set; }
        public Stock Stock { get; set; }
        public User User { get; set; }

    }

    public class HoldingDetails
    {
        public ulong PurchaseId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
