using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Connection
{
    public class ConnectionService
    {
       public MySqlConnection GetConnection()
       {
            var connection = new MySqlConnection();
            connection.ConnectionString = GetConnectionString();

            return connection;
       }

        private string GetConnectionString()
        {
            return string.Empty;
        }
    }
}
