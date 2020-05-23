using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Persistence.UserData
{
    public interface IUserData
    {
        Task<ulong> AddUser(User user, IDbConnection connection);
        Task<bool> CheckIfUsernameExists(string username, IDbConnection connection);
    }
}
