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
    }
}
