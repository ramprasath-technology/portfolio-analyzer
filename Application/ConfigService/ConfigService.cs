using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ConfigService
{
    public class ConfigService : IConfigService
    {
        private IConfiguration _configuration;

        public ConfigService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetCompanyProfileUrl()
        {
            return _configuration.GetSection("URLs").GetSection("FinancialModelingPrepProfile").Value;
        }

        public string GetDailyPriceUrl()
        {
            return _configuration.GetSection("URLs").GetSection("FinancialModelingPrepDailyPrice").Value;
        }

        public string GetStockQuoteUrl()
        {
            return _configuration.GetSection("URLs").GetSection("FinancialModelingPrepLastQuotePrice").Value;
        }

        public string GetFinancialModelingPrepKey()
        {
            return _configuration.GetSection("APIKeys").GetSection("FinancialModelingPrep").Value;
        }

        public string GetCNNStockInfoUrl()
        {
            return _configuration.GetSection("URLs").GetSection("CNNStockInformation").Value;
        }
    }
}
