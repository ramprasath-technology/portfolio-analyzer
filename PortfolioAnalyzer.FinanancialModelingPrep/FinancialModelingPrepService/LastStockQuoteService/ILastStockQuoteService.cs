using Domain.DTO.ExternalData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService.LastStockQuoteService
{
    public interface ILastStockQuoteService
    {
        Task<IEnumerable<LastStockQuote>> GetLastQuoteForStocks(string baseUrl, string apiKey, IEnumerable<string> tickers);
    }
}
