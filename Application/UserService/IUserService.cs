using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserService
{
    public interface IUserService
    {
        Task<ulong> AddUser(User user);
        Task<bool> CheckIfUsernameExists(string username);
    }
}
