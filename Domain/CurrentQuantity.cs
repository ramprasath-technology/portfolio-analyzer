using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class CurrentQuantity
    {
        public ulong CurrentQuantityId { get; set; }
        public ulong StockId { get; set; }
        public ulong UserId { get; set; }
        public uint Quantity { get; set; }
        public uint ShardNumber { get; set; }
        public User User { get; set; }
        public Stock Stock { get; set; }
    }
}
