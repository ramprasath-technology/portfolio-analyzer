using Application.StockHoldingService;
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
        private readonly IStockHoldingService _stockHoldingService;

        public StockAndPurchaseService(IStockService stockService, IStockPurchaseService stockPurchaseService, IStockHoldingService stockHoldingService)
        {
            _stockService = stockService;
            _stockPurchaseService = stockPurchaseService;
            _stockHoldingService = stockHoldingService;
        }

        public async Task<StockPurchase> AddStockAndPurchaseInfo(StockPurchase stockPurchase)
        {
            stockPurchase.Stock = await AddStockInfo(stockPurchase);
            stockPurchase.StockId = stockPurchase.Stock.StockId;
            stockPurchase.Purchase = await AddPurchaseInfo(stockPurchase.StockId, stockPurchase);

            return stockPurchase;
        }

        private async Task<Stock> AddStockInfo(StockPurchase stockPurchase)
        {
            var stock = await _stockService.GetStockByTicker(stockPurchase.UserId, stockPurchase.Ticker);

            if (stock == null)
            {
                var companyProfile = await _stockService.GetCompanyProfile(stockPurchase.Ticker);
                if (companyProfile == null || companyProfile.Profile == null)
                {
                    throw new ArgumentException($"Ticket symbol is not valid");
                }

                stock = new Stock();
                stock.Ticker = stockPurchase.Ticker;
                stock.CompanyName = companyProfile.Profile.CompanyName;
                await _stockService.AddStock(stockPurchase.UserId, stock);
            }

            return stock;
        }

        private async Task<Purchase> AddPurchaseInfo(ulong stockId, StockPurchase stockPurchase)
        {
            var purchase = new Purchase();
            purchase.Comment = stockPurchase.Comment;
            purchase.Date = stockPurchase.PurchaseDate;
            purchase.Price = stockPurchase.Price;
            purchase.Quantity = stockPurchase.Quantity;
            purchase.StockId = stockId;
            purchase.UserId = stockPurchase.UserId;

            purchase = await _stockPurchaseService.AddStockPurchase(stockPurchase.UserId, purchase);

            return purchase;
        }
    }
}
