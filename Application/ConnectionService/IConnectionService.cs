using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Application.Connection
{
    public interface IConnectionService
    {
        string GetAlphaVantageAPIKey();
        IDbConnection GetConnection(ulong userId);
    }
}
