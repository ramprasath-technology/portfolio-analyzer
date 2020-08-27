using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ConfigService
{
    public interface IConfigService
    {
        string GetCompanyProfileUrl();
        string GetDailyPriceUrl();
        string GetStockQuoteUrl();
        string GetFinancialModelingPrepKey();
        string GetCNNStockInfoUrl();
    }
}
