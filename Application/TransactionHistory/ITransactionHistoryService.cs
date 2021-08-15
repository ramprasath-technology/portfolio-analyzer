using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.TransactionHistory
{
    public interface ITransactionHistoryService
    {
        Task ImportTransactionHistory(ulong userId, string fileName);
    }
}
