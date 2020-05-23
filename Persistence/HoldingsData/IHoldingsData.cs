using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.HoldingsData
{
    public interface IHoldingsData
    {
        Task<IEnumerable<Holdings>> GetAllHoldingsForUser(IDbConnection connection, ulong userId);
        /// <summary>
        /// Gets holding data of a particular stock for a particular user
        /// </summary>
        /// <param name="connection" type="IDBConnection">DB Connection</param>
        /// <param name="userId" type="ulong">User Id</param>
        /// <param name="stockId" type="ulong">Stock Id</param>
        /// <returns type="Holdings">Holding Data</returns>
        Task<Holdings> GetHoldingDataForUserAndStock(IDbConnection connection, ulong userId, ulong stockId);
        /// <summary>
        /// Add new holding data
        /// </summary>
        /// <param name="connection">DB Connection</param>
        /// <param name="holdings">Holding details</param>
        /// <returns></returns>
        Task<ulong> AddHolding(IDbConnection connection, Holdings holdings);
        /// <summary>
        /// Update holding details for a particular user and particular stock
        /// </summary>
        /// <param name="connection">DB Connection</param>
        /// <param name="holdingId">Holding Id</param>
        /// <param name="holdingDetails">Holding Details</param>
        /// <returns>Holding Id</returns>
        Task UpdateHoldingDetail(IDbConnection connection, ulong holdingId, IEnumerable<HoldingDetails> holdingDetails);
        /// <summary>
        /// Check if holding exists for an user and a stock
        /// </summary>
        /// <param name="connection">DB Connection</param>
        /// <param name="userId">User Id</param>
        /// <param name="stockId">Stock Id</param>
        /// <returns>True or False</returns>
        Task<bool> CheckIfHoldingExists(IDbConnection connection, ulong userId, ulong stockId);
        Task DeleteHolding(IDbConnection connection, ulong holdingId);
    }
}
