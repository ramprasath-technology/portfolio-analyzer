﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Purchase
    {
        public ulong PurchaseId { get; set; }
        public ulong UserId { get; set; }
        public ulong StockId { get; set; }
        public uint Quantity { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public uint ShardNumber { get; set; }
        public User User { get; set; }
        public Stock Stock { get; set; }
        

    }
}
