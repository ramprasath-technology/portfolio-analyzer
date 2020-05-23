using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepDTO;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService
{
    public class CompanyProfileService : ICompanyProfileService
    {
        public async Task<CompanyProfile> GetCompanyProfile(string url, string apiKey, string ticker)
        {
            var completeUrl = $"{url}{ticker}?apikey={apiKey}";
            var client = new HttpClient();
            var companyProfileJson = await client.GetStreamAsync(completeUrl);
            var companyProfile = await JsonSerializer.DeserializeAsync<CompanyProfile>(companyProfileJson);
            return companyProfile;
        }    
    }
}
