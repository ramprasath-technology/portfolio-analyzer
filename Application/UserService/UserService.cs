using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Connection;
using Domain;
using Persistence.UserData;

namespace Application.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserData _userData;
        private readonly IConnectionService _connectionService; 

        public UserService(IUserData userData, IConnectionService connectionService)
        {
            _userData = userData;
            _connectionService = connectionService;
        }

        public async Task<ulong> AddUser(User user)
        {
            var usernameExists = await CheckIfUsernameExists(user.UserName);
            if(!usernameExists)
            {
                var userId = await _userData.AddUser(user, _connectionService.GetConnection(0));
                return userId;
            }
            else
            {
                return 0;
            }
            
        }

        public async Task<bool> CheckIfUsernameExists(string username)
        {
            var connectionToCommonShard = _connectionService.GetConnectionToCommonShard();
            var usernameExists = await _userData.CheckIfUsernameExists(username, connectionToCommonShard);
            return usernameExists;
        }
    }
}
