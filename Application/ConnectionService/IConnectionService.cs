using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Application.Connection
{
    public interface IConnectionService
    {
        /// <summary>
        /// Gets secret key to AlphaVantage API
        /// </summary>
        /// <returns>AlphaVantage key</returns>
        string GetAlphaVantageAPIKey();
        /// <summary>
        /// Gets database connection to a particular shard depending on user id
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Database Connection Object</returns>
        IDbConnection GetConnection(ulong userId);
        /// <summary>
        /// Gets database connection to the common shard
        /// </summary>
        /// <returns>Database Connection Object</returns>
        IDbConnection GetConnectionToCommonShard();
        /// <summary>
        /// Dispose a connection
        /// </summary>
        /// <param name="connection">DB Connection</param>
        void DisposeConnection(IDbConnection connection);
        IDbConnection GetOpenConnection(ulong userId);
    }
}
