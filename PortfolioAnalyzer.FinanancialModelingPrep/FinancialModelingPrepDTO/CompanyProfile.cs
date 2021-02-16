using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepDTO
{
    public class CompanyProfile
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("profile")]
        public Profile Profile { get; set; }
    }

    public class Profile
    {
        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }
        [JsonPropertyName("industry")]
        public string Industry { get; set; }
        [JsonPropertyName("sector")]
        public string Sector { get; set; }
        [JsonPropertyName("country")]
        public string Country { get; set; }
    }
}
