using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockQuoteService
{
    public interface IStockQuoteService
    {
        Task<Dictionary<string, decimal>> GetLatestQuoteForStocks(IEnumerable<string> tickers);
    }
}
