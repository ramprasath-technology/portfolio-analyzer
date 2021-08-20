using Domain.DTO.ExternalData;
using PortfolioAnalyzer.Alphavantage.DTO;
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
            var companyProfile = new CompanyProfile();
            var completeUrl = $"{url}&symbol={ticker}&apikey={apiKey}";
            var client = new System.Net.WebClient();
            var alphaVantageModel = JsonSerializer.Deserialize<AlphaVantageCompanyProfile>(client.DownloadString(completeUrl));
            MapAlphaVantageModelToCompanyProfile(alphaVantageModel, companyProfile);
            /*
           if (json_data != null)
            {
                var tasks = new List<Task>();
                foreach (var property in json_data)
                {
                   tasks.Add(Task.Run(() =>
                   {
                       MapProperties(property, companyProfile);
                   }));
                }
                Task.WaitAll(tasks.ToArray());
            }
            */
            return companyProfile;
        }

        private void MapAlphaVantageModelToCompanyProfile(AlphaVantageCompanyProfile alphaVantageModel,
            CompanyProfile companyProfile)
        {
            companyProfile.Symbol = alphaVantageModel.Symbol;
            companyProfile.Profile.CompanyName = alphaVantageModel.Name;
            companyProfile.Profile.Country = alphaVantageModel.Country;
            companyProfile.Profile.Industry = alphaVantageModel.Industry;
            companyProfile.Profile.Sector = alphaVantageModel.Sector;
        }

        /*
        private void MapProperties(dynamic property, CompanyProfile companyProfile)
        {
            MapSymbol(property, companyProfile);
            MapCompanyName(property, companyProfile);
            MapIndustry(property, companyProfile);
            MapSector(property, companyProfile);
            MapCountry(property, companyProfile);
        }

        private void MapSymbol(dynamic property, CompanyProfile companyProfile)
        {
            if (property != null &&
                property.Key != null &&
                property.Key == "Symbol")
            {
                companyProfile.Symbol = property.Value;
            }
        }

        private void MapCompanyName(dynamic property, CompanyProfile companyProfile)
        {
            if (property != null &&
                property.Key != null &&
                property.Key == "Name")
            {
                companyProfile.Profile.CompanyName = property.Value;
            }
        }

        private void MapIndustry(dynamic property, CompanyProfile companyProfile)
        {
            if (property != null &&
                property.Key != null &&
                property.Key == "Industry")
            {
                companyProfile.Profile.Industry = property.Value;
            }
        }

        private void MapSector(dynamic property, CompanyProfile companyProfile)
        {
            if (property != null &&
                property.Key != null &&
                property.Key == "Sector")
            {
                companyProfile.Profile.Sector = property.Value;
            }
        }

        private void MapCountry(dynamic property, CompanyProfile companyProfile)
        {
            if (property != null &&
                property.Key != null &&
                property.Key == "Country")
            {
                companyProfile.Profile.Country = property.Value;
            }
        }
        */
    }
}
