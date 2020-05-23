using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Sale
    {
        public ulong SaleId { get; set; }
        public ulong UserId { get; set; }
        public ulong StockId { get; set; }
        public ulong PurchaseId { get; set; }
        public uint Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public DateTime DateAdded { get; set; }
        public string Username { get; set; }
        public Purchase Purchase { get; set; }
        public User User { get; set; }
        public Stock Stock { get; set; }
    }
}
