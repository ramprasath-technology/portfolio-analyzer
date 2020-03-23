using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Application.Connection
{
    public class ConnectionService : IConnectionService
    {
        private IConfiguration _configuration;

        public ConnectionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

       public IDbConnection GetConnection(ulong userId)
       {
            var connection = new MySqlConnection();
            connection.ConnectionString = GetConnectionString(userId);
            return connection;
       }

        private string GetConnectionString(ulong userId)
        {
            var connectionStringName = $"ConnectionString{GetShardNumber(userId)}";
            return _configuration.GetSection("DataBase").GetSection(connectionStringName).Value;
        }

        private uint GetShardNumber(ulong userId)
        {
            /*
             * In production scenario, we get the shard number based on the userId
             * For example, if userId is between 0 and 100000, the shard number would be 1
             * If we're creating a new user, then the current active shard would be returned
             */
            if (userId > 0)
                return 1;

            return GetNextAvailableShard();
        }

        private uint GetNextAvailableShard()
        {
            return 1;
        }

        public string GetAlphaVantageAPIKey()
        {
            return _configuration.GetSection("APIKeys").GetSection("AlphaVantage").Value;
        }
    }
}
