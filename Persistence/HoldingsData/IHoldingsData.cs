using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.HoldingsData
{
    public interface IHoldingsData
    {
        /// <summary>
        /// Gets all current and historic holdings for a user
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="shardNumber">Shard Number</param>
        /// <returns>An object containing stock, purchase, and sale data</returns>
        Domain.Holdings GetAllHoldings(ulong userId, uint shardNumber);
    }
}
