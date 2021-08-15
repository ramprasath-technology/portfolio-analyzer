using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ConfigService
{
    public interface IConfigService
    {
        string GetFinancialModelingPrepKey();
        string GetCNNStockInfoUrl();
        string GetExternalDataServiceName();
        string GetFidelityFilePath();
    }
}
