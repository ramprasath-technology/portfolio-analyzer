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

        public async Task<Stock> AddStock(ulong userId, Stock stock)
        {
            var connection = _connectionService.GetConnection(userId);

            await _stockData.AddStock(_connectionService.GetConnection(userId), stock);

            _connectionService.DisposeConnection(connection);

            return stock;
        }

        public async Task<Stock> GetStockByTicker(ulong userId, string ticker)
        {
            using (var connection = _connectionService.GetOpenConnection(userId))
            {
                var stock = await _stockData.GetStockByTicker(connection, ticker);
                return stock;
            }
        }

        public async Task<ulong> GetStockIdByTicker(ulong userId, string ticker)
        {
            var stockId = await _stockData.GetStockIdByTicker(ticker, _connectionService.GetConnection(userId));

            return stockId;
        }

        public async Task<CompanyProfile> GetCompanyProfile(string ticker)
        {
            var url = _configService.GetCompanyProfileUrl();
            var apiKey = _configService.GetFinancialModelingPrepKey();
            var companyProfile = await _companyProfileService.GetCompanyProfile(url, apiKey, ticker);

            return companyProfile;
        }

        public async Task<IEnumerable<Stock>> GetStocksById(ulong userId, IEnumerable<ulong> stockId)
        {
            var connection = _connectionService.GetConnection(userId);

            var stocks = await _stockData.GetStocksById(connection, stockId);

            _connectionService.DisposeConnection(connection);

            return stocks;
        }
    }
}
