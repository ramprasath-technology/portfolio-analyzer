using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class StockTransaction
    {
        public ulong TransactionId { get; set; }
        public ulong StockId { get; set; }
        public ulong UserId { get; set; }
        public ulong PurchaseId { get; set; }
        public ulong SalesId { get; set; }
        public uint ShardNumber { get; set; }
    }
}
