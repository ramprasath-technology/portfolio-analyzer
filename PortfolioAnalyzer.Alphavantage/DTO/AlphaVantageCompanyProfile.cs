using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAnalyzer.Alphavantage.DTO
{
    public class AlphaVantageCompanyProfile
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }
    }
}
