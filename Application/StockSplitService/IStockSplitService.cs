using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockSplitService
{
    public interface IStockSplitService
    {
        Task PerformStockSplit(ulong userId, StockSplit splitDetails);
    }
}
