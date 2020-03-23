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
        [Range(0.1, 100000.00)]
        public decimal Price { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
        [Required]
        [Range(0.1, Double.MaxValue)]
        public decimal Quantity { get; set; }
        public string Comment { get; set; }
    }
}
