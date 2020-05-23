using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class StockIndexTicker
    {
        public int TickerId { get; set; }
        [Required]
        [MaxLength(45)]
        public string Ticker { get; set; }
        [Required]
        [MaxLength(100)]
        public string TickerDescription { get; set; }
    }
}
