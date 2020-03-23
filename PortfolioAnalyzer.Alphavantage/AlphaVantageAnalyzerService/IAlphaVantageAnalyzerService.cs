using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThreeFourteen.AlphaVantage.Builders.Stocks;

namespace PortfolioAnalyzer.AlphavantageAnalyzerService
{
    public interface IAlphaVantageAnalyzerService
    {
        Task<string> GetLatestQuoteForStock(string ticker, string apiKey);
    }
}
