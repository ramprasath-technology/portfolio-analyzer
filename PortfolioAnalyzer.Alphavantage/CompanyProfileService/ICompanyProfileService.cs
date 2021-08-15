using Domain.DTO.ExternalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAnalyzer.Alphavantage.CompanyProfileService
{
    public interface ICompanyProfileService
    {
        CompanyProfile GetCompanyProfile(string url, string apiKey, string ticker);
    }
}
