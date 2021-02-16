using Application.ConfigService;
using Application.Connection;
using Domain;
using Persistence.StockData;
using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepDTO;
using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public async Task<IEnumerable<Stock>> GetStocks(IDbConnection connection = null)
        {
            var dbConnection = connection == null ? _connectionService.GetConnectionToCommonShard() : connection;
            var stocks = await _stockData.GetStocks(connection);
            if (dbConnection != connection)
                dbConnection.Dispose();

            return stocks;
        }

        public async Task UpdateCompanyProfile(IDbConnection connection = null)
        {
            var dbConnection = connection == null ? _connectionService.GetConnectionToCommonShard() : connection;
            var stocks = await GetStocks(dbConnection);
            var stockTickerStockMap = stocks.ToDictionary(x => x.Ticker);
            var companyProfileTasks = new List<Task<CompanyProfile>>();
            var stockTasks = new List<Task>();

            foreach (var stock in stocks)
            {
                companyProfileTasks.Add(GetCompanyProfile(stock.Ticker));
                await Task.Delay(100);
            }

            var companiesProfile = await Task.WhenAll(companyProfileTasks);

            foreach (var companyProfile in companiesProfile)
            {
                Stock stock;
                if (stockTickerStockMap.TryGetValue(companyProfile.Symbol, out stock))
                {
                    stock.CompanyName = companyProfile.Profile.CompanyName;
                    stock.Country = companyProfile.Profile.Country;
                    stock.Industry = companyProfile.Profile.Industry;
                    stock.Sector = companyProfile.Profile.Sector;
                    stockTasks.Add(_stockData.UpdateStock(stock, connection));
                }                
            }

            await Task.WhenAll(stockTasks);
        }
    }
}
