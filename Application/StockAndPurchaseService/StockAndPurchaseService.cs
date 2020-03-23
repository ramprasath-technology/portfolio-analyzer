using Application.StockPurchaseService;
using Application.StockService;
using Domain;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockAndPurchaseService
{
    public class StockAndPurchaseService : IStockAndPurchaseService
    {
        private readonly IStockService _stockService;
        private readonly IStockPurchaseService _stockPurchaseService;

        public StockAndPurchaseService(IStockService stockService, IStockPurchaseService stockPurchaseService)
        {
            _stockService = stockService;
            _stockPurchaseService = stockPurchaseService;
        }

        public async Task AddStockAndPurchaseInfo(StockPurchase stockPurchase)
        {
            var stockId = await _stockService.GetStockIdByTicker(stockPurchase.UserId, stockPurchase.Ticker);

            if(stockId == 0)
            {
                var companyProfile = await _stockService.GetCompanyProfile(stockPurchase.Ticker);
                if(companyProfile == null || companyProfile.Profile == null)
                {
                    throw new ArgumentException($"Ticket symbol is not valid");
                }

                var stock = new Stock();
                stock.Ticker = stockPurchase.Ticker;
                stock.CompanyName = companyProfile.Profile.CompanyName;
                stockId = await _stockService.AddStock(stockPurchase.UserId, stock);
            }          

            var purchase = new Purchase();
            purchase.Comment = stockPurchase.Comment;
            purchase.Date = stockPurchase.PurchaseDate;
            purchase.Price = stockPurchase.Price;
            purchase.Quantity = stockPurchase.Quantity;
            purchase.StockId = stockId;
            purchase.UserId = stockPurchase.UserId;

            await _stockPurchaseService.AddStockPurchase(stockPurchase.UserId, purchase);
        }
    }
}
