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
        public ulong PurchaseId { get; set; }
        public ulong SaleId { get; set; }
        public uint ShardNumber { get; set; }
        public Stock Stock { get; set; }
        public User User { get; set; }
        public Purchase Purchase { get; set; }
        public Sale Sale { get; set; }

    }
}
