using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ConfigService
{
    public interface IExternalServiceConfig
    {
        string GetCompanyProfileUrl();
        string GetDailyPriceUrl();
        string GetStockQuoteUrl();
        string GetKey();
    }
}
