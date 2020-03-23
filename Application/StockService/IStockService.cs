using Domain;
using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockService
{
    public interface IStockService
    {
        Task<ulong> AddStock(ulong userId, Stock stock);
        Task<CompanyProfile> GetCompanyProfile(string ticker);
        Task<ulong> GetStockIdByTicker(ulong userId, string ticker);
    }
}
