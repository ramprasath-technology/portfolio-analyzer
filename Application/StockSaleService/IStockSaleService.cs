using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockSaleService
{
    public interface IStockSaleService
    {
        Task<Sale> AddSale(ulong userId, Sale sale);
    }
}
