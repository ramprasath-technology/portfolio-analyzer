using Domain.DTO.ExternalData;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ThreeFourteen.AlphaVantage;

namespace PortfolioAnalyzer.Alphavantage.CompanyProfileService
{
    public class CompanyProfileService : ICompanyProfileService
    {
        public CompanyProfile GetCompanyProfile(string url, string apiKey, string ticker)
        {
            var completeUrl = $"{url}&symbol={ticker}&apikey={apiKey}";
            var client = new System.Net.WebClient();
            dynamic json_data = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(completeUrl));
            return new CompanyProfile();
        }
    }
}
