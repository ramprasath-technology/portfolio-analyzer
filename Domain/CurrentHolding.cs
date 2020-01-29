using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class CurrentHolding
    {
        public long HoldingId { get; set; }
        public long StockId { get; set; }
        public long UserId { get; set; }
        public int Quantity { get; set; }
        public Stock Stock { get; set; }
        public User User { get; set; }
        public int ShardNumber { get; set; }
    }
}
