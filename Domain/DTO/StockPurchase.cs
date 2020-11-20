using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DTO
{
    public class StockPurchase
    {
        [Required]
        public ulong UserId { get; set; }
        [Required]
        public string Ticker { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
        [Required]
        public decimal Quantity { get; set; }

        public string Comment { get; set; }
        public ulong StockId { get; set; }
        public ulong PurchaseId { get; set; }
        public Stock Stock { get; set; }
        public Purchase Purchase { get; set; }
    }
}
