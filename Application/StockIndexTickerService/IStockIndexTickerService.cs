using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockIndexTickerService
{
    public interface IStockIndexTickerService
    {
        /// <summary>
        /// Check if index ticker exists
        /// </summary>
        /// <param name="ticker">Ticker</param>
        /// <returns>True or False</returns>
        Task<bool> CheckIfStockTickerExists(string ticker);
        Task<StockIndexTicker> GetStockIndex(string ticker);
    }
}
