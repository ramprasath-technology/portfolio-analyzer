using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ConfigService
{
    public class FinancialModelPrepServiceConfig : IExternalServiceConfig
    {
        private IConfiguration _configuration;

        public FinancialModelPrepServiceConfig(IConfiguration configuration)
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

        public string GetKey()
        {
            return _configuration.GetSection("APIKeys").GetSection("FinancialModelingPrep").Value;
        }

        public string GetStockQuoteUrl()
        {
            return _configuration.GetSection("URLs").GetSection("FinancialModelingPrepLastQuotePrice").Value;
        }
    }
}
