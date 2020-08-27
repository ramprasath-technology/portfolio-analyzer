using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class StockWatchlist
    {
        public uint WatchlistId { get; set; }
        public string WatchlistName { get; set; }
        public ulong UserId { get; set; }
        public ulong StockId { get; set; }
        public uint WatchlistTypeId { get; set; }
        public StockWatchlistType StockWatchlistType { get; set; }
        public User User { get; set; }
        public Stock Stock { get; set; }
    }
}
