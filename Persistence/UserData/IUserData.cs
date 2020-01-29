using System;
using System.Collections.Generic;
using System.Text;
using Domain;

namespace Persistence.UserData
{
    interface IUserData
    {
        void addUser(User user);
        User GetUserById(long id);
    }
}
