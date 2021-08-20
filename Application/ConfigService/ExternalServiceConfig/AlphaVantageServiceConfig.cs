using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ConfigService
{
    public class AlphaVantageServiceConfig : IExternalServiceConfig
    {      
        private IConfiguration _configuration;

        public AlphaVantageServiceConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetCompanyProfileUrl()
        {
            return _configuration.GetSection("URLs").GetSection("AlphaVantageProfile").Value;
        }

        public string GetDailyPriceUrl()
        {
            throw new NotImplementedException();
        }

        public string GetKey()
        {
            return _configuration.GetSection("APIKeys").GetSection("AlphaVantage").Value;
        }

        public string GetStockQuoteUrl()
        {
            return _configuration.GetSection("URLs").GetSection("AlphaVantageStockQuote").Value;
        }
    }
}
