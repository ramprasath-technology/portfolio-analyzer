using Application.ConfigService;
using Application.Connection;
using Domain;
using Persistence.StockData;
using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepDTO;
using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockService
{
    public class StockService : IStockService
    {
        private readonly IStockData _stockData;
        private readonly IConfigService _configService;
        private readonly IConnectionService _connectionService;
        private readonly ICompanyProfileService _companyProfileService;
        
        public StockService(IStockData stockData, IConfigService configService, IConnectionService connectionService, ICompanyProfileService companyProfileService)
        {
            _stockData = stockData;
            _connectionService = connectionService;
            _configService = configService;
            _companyProfileService = companyProfileService;
        }

        public async Task<ulong> AddStock(ulong userId, Stock stock)
        {
            var stockId = await _stockData.AddStock(stock, _connectionService.GetConnection(userId));

            return stockId;
        }

        public async Task<ulong> GetStockIdByTicker(ulong userId, string ticker)
        {
            var stockId = await _stockData.GetStockIdByTicker(ticker, _connectionService.GetConnection(userId));

            return stockId;
        }

        public async Task<CompanyProfile> GetCompanyProfile(string ticker)
        {
            var url = _configService.GetCompanyProfileUrl();
            var companyProfile = await _companyProfileService.GetCompanyProfile(url, ticker);

            return companyProfile;
        }
    }
}
