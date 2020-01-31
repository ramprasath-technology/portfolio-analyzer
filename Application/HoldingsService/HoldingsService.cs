using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Persistence.HoldingsData;

namespace Application.HoldingsService
{
    public class HoldingsService : IHoldingsService
    {
        private IHoldingsData _holdingsData;

        public HoldingsService(IHoldingsData holdingsData)
        {
            _holdingsData = holdingsData;
        }

        public List<Holdings> GetHoldingsForUser(ulong userId)
        {
            uint shardNumber = 1;
            return _holdingsData.GetAllHoldings(userId, shardNumber);
        }
    }
}
