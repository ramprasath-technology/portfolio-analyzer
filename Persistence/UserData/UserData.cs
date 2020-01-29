using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.UserData
{
    public class UserData
    {
        private const string _addUser =
            @"INSERT INTO portfolio_analyzer.user_user (user_name, shard_number) VALUES (?userName, ?shardNumber)";

        private const string _getUserById =
            @"SELECT 
                *
            FROM
                portfolio_analyzer.user_user
            WHERE
                user_id = ?userId";


        
    }
}
