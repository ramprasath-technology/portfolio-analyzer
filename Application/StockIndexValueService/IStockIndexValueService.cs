﻿using Domain;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.StockIndexValueService
{
    public interface IStockIndexValueService
    {
        Task AddIndexValues(StockIndexValueInputs stockIndexValueInputs);
        Task<IEnumerable<StockIndexValue>> GetPricesForGivenIndexAndDate(ulong userId, IEnumerable<string> ticker, IEnumerable<DateTime> date);
    }
}
