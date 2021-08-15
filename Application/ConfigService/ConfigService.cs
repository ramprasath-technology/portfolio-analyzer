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

        public string GetFinancialModelingPrepKey()
        {
            return _configuration.GetSection("APIKeys").GetSection("FinancialModelingPrep").Value;
        }

        public string GetCNNStockInfoUrl()
        {
            return _configuration.GetSection("URLs").GetSection("CNNStockInformation").Value;
        }

        public string GetExternalDataServiceName()
        {
            return _configuration.GetSection("ExternalServices").GetSection("CurrentStockDataService").Value;
        }

        public string GetFidelityFilePath()
        {
            return _configuration.GetSection("TransactionFilePath").GetSection("Fidelity").Value;
        }
    }
}
