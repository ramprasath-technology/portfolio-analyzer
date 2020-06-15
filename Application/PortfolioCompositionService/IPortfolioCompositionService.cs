using Domain.DTO.StockAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.PortfolioCompositionService
{
    public interface IPortfolioCompositionService
    {
        Task<PortfolioComposition> GetPortfolioComposition(ulong userId);
    }
}
