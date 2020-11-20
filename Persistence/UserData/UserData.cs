using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Persistence.UserData
{
    public class UserData : IUserData
    {
        #region Queries
        private const string insUser =
            @"INSERT INTO user_user(user_user_name)
                VALUES (?userName);
            SELECT LAST_INSERT_ID();";

        private const string selUsername =
            @"SELECT EXISTS
              (SELECT 1
                 FROM user_username u
                WHERE u.username = ?username
                LIMIT 1)";
        #endregion

        public async Task<ulong> AddUser(User user, IDbConnection connection)
        {
            connection.Open();

            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("userName", user.UserName);

            
            var userId = await connection.QueryFirstAsync<ulong>(insUser, dynamicParams);

            return userId;
        }

        public async Task<bool> CheckIfUsernameExists(string username, IDbConnection connection)
        {
            connection.Open();

            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("username", username);

            var userExists = await connection.QueryFirstAsync<bool>(selUsername, dynamicParams);

            return userExists;
        }




    }
}
