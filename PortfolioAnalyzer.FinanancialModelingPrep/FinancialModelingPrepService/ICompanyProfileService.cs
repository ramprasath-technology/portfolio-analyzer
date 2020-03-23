﻿using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService
{
    public interface ICompanyProfileService
    {
        Task<CompanyProfile> GetCompanyProfile(string url, string tickers);
    }
}
