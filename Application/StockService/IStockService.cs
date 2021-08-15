using Domain;
using Domain.DTO.ExternalData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockService
{
    public interface IStockService
    {
        Task<Stock> AddStock(ulong userId, Stock stock);
        Task<CompanyProfile> GetCompanyProfile(string ticker);
        Task<ulong> GetStockIdByTicker(ulong userId, string ticker);
        Task<Stock> GetStockByTicker(ulong userId, string ticker);
        Task<IEnumerable<Stock>> GetStocksById(ulong userId, IEnumerable<ulong> stockId);
        Task UpdateCompanyProfile(IDbConnection connection = null);
    }
}
