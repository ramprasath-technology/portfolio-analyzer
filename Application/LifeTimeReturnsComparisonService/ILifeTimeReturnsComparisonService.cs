using Domain.DTO.StockAnalysis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Application.LifeTimeReturnsComparisonService
{
    public interface ILifeTimeReturnsComparisonService
    {
        Task<LifeTimeComparison> GetLifeTimeComparison(ulong userId,
            IDbConnection connection = null);
    }
}
