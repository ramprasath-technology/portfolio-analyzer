using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Persistence.HoldingsData;
using System.Threading.Tasks;
using Application.Connection;
using System.Linq;
using System.Text.Json;
using System.Data;

namespace Application.StockHoldingService
{
    public class StockHoldingService : IStockHoldingService
    {
        private IHoldingsData _holdingsData;
        private IConnectionService _connectionService;

        public StockHoldingService(IHoldingsData holdingsData, IConnectionService connectionService)
        {
            _holdingsData = holdingsData;
            _connectionService = connectionService;
        }

        public async Task<IEnumerable<Holdings>> GetAllHoldingsForUser(ulong userId)
        {
            using (var connection = _connectionService.GetConnection(userId))
            {
                var holdings = await _holdingsData.GetAllHoldingsForUser(connection, userId);
                return holdings;
            }                
        }

        public async Task<IEnumerable<Holdings>> GetAllHoldingsForUserWithStockDetails(ulong userId)
        {
            using (var connection = _connectionService.GetConnection(userId))
            {
                var holdings = await _holdingsData.GetAllHoldingsForUserWithStockDetails(connection, userId);
                return holdings;
            }
        }

        public async Task AddPurchaseToHoldings(ulong userId, Purchase purchase)
        {
            var connection = _connectionService.GetConnection(userId);
            var stockId = purchase.StockId;
            var holdingExists = await _holdingsData.CheckIfHoldingExists(connection, userId, stockId);
            var holdingDetails = ConstructHoldingDetailsFromPurchaseData(purchase);

            if (holdingExists)
            {
                await UpdateHoldingDetails(connection, userId, stockId, holdingDetails);
            }
            else
            {
                await AddNewHoldingDetails(connection, userId, stockId, holdingDetails);
            }

            _connectionService.DisposeConnection(connection);
        }

        public IEnumerable<ulong> GetPurchaseIdsFromHolding(IEnumerable<Holdings> holdings)
        {
            var purchaseIds = new HashSet<ulong>();

            foreach (var holding in holdings)
            {
                var holdingsDetail = holding.HoldingDetails;

                foreach (var holdingDetail in holdingsDetail)
                {
                    purchaseIds.Add(holdingDetail.PurchaseId);
                }
            }

            return purchaseIds;
        }

        public async Task UpdateHoldingDetails(ulong userId, Holdings holding)
        {
            using (var connection = _connectionService.GetOpenConnection(userId))
            {
                var holdingDetail = holding.HoldingDetails;
                await _holdingsData.UpdateHoldingDetail(connection, holding.HoldingId, holdingDetail);
            }
        }

        private async Task AddNewHoldingDetails(IDbConnection connection, ulong userId, ulong stockId, HoldingDetails holdingDetails)
        {
            var holdings = new Holdings()
            {
                HoldingDetails = new List<HoldingDetails>() { holdingDetails },
                StockId = stockId,
                UserId = userId
            };

            var holdingId = await _holdingsData.AddHolding(connection, holdings);
        }

        private async Task UpdateHoldingDetails(IDbConnection connection, ulong userId, ulong stockId, HoldingDetails holdingDetails)
        {
            var holdings = await _holdingsData.GetHoldingDataForUserAndStock(connection, userId, stockId);
            var currentHolding = holdings.HoldingDetails.ToList();
            if (!CheckIfPurchaseIdExists(holdingDetails.PurchaseId, currentHolding))
            {
                currentHolding.Add(holdingDetails);
                await _holdingsData.UpdateHoldingDetail(connection, holdings.HoldingId, currentHolding);
            }
        }

        private bool CheckIfPurchaseIdExists(ulong purchaseId, IEnumerable<HoldingDetails> holdingDetails)
        {
            var holding = holdingDetails.FirstOrDefault(x => x.PurchaseId == purchaseId);
            if(holding != null)
            {
                return true;
            }

            return false;
        }

        private HoldingDetails ConstructHoldingDetailsFromPurchaseData(Purchase purchase)
        {
            var newHolding = new HoldingDetails()
            {
                Price = purchase.Price,
                PurchaseId = purchase.PurchaseId,
                Quantity = purchase.Quantity
            };

            return newHolding;
        }

        public async Task UpdateHoldingAfterSale(ulong userId, Sale sale)
        {
            var newHoldings = new List<HoldingDetails>();
            var connection = _connectionService.GetConnection(userId);

            var holdings = await _holdingsData.GetHoldingDataForUserAndStock(connection, userId, sale.StockId);

            foreach(var holding in holdings.HoldingDetails)
            {
                if(holding.PurchaseId == sale.PurchaseId)
                {
                    holding.Quantity -= sale.Quantity;
                    if(holding.Quantity > 0)
                    {
                        newHoldings.Add(holding);
                    }
                }
                else
                {
                    newHoldings.Add(holding);
                }
            }

            if (newHoldings.Count == 0)
                await DeleteStockHolding(connection, holdings.HoldingId);
            else
                await _holdingsData.UpdateHoldingDetail(connection, holdings.HoldingId, newHoldings);

            _connectionService.DisposeConnection(connection);
        }

        private async Task DeleteStockHolding(IDbConnection connection, ulong holdingId)
        {
            await _holdingsData.DeleteHolding(connection, holdingId);
        }

        private async Task AddToExistingHoldings(IDbConnection connection, Purchase purchase)
        {
            /*var holdings = await _holdingsData.GetHoldingDetailsForParticularStock(connection, userId, stockId);
            if (holdings == null)
            {

            }*/
        }

    }
}
