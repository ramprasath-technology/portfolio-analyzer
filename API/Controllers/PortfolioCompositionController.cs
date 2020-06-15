using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.PortfolioCompositionService;
using Domain.DTO.StockAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioCompositionController : ControllerBase
    {
        private readonly IPortfolioCompositionService _portfolioCompositionService;

        public PortfolioCompositionController(IPortfolioCompositionService portfolioCompositionService)
        {
            _portfolioCompositionService = portfolioCompositionService;
        }

        [HttpGet("GetPortfolioComposition/{userId}")]
        public async Task<PortfolioComposition> GetPortfolioComposition(ulong userId)
        {
            var portfolioComposition = await _portfolioCompositionService.GetPortfolioComposition(userId);
            return portfolioComposition;
        }
    }
}