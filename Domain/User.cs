using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class User
    {
        public ulong UserId { get; set; }
        public string UserName { get; set; }
        public uint ShardNumber { get; set; }
    }
}
