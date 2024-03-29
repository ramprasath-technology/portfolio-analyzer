﻿using Domain.DTO.ExternalData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService
{
    public interface IDailyStockPriceService
    {
        Task<DailyStockPrice> GetDailyStockPriceService(string baseUrl, string ticker, string apiKey, DateTime startDate, DateTime endDate);
    }
}
