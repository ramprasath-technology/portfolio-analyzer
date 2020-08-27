using Domain.DTO.StockAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.CompositionAndRecommendationService
{
    public interface ICompositionAndRecommendationService
    {
        Task<CompositionAndRecommendation> GetCompositionAndRecommendation(ulong userId);
    }
}
