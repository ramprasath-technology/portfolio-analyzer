using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.CompositionAndRecommendationService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompositionAndRecommendationController : ControllerBase
    {
        private readonly ICompositionAndRecommendationService _compositionAndRecommendationService;
        public CompositionAndRecommendationController(ICompositionAndRecommendationService compositionAndRecommendationService)
        {
            _compositionAndRecommendationService = compositionAndRecommendationService;
        }

        [HttpGet("GetCompositionAndRecommendation/{userId}/{percentageFromMedian?}/{percentageFromFiftyTwoWeekHigh?}/{portfolioComposition?}")]
        public async Task<IActionResult> GetCompositionAndRecommendation(ulong userId, 
            decimal percentageFromMedian, 
            decimal percentageFromFiftyTwoWeekHigh, 
            decimal portfolioComposition)
        {
            var compositionAndRecommendation = await _compositionAndRecommendationService.GetCompositionAndRecommendation(userId);
            return Ok(compositionAndRecommendation);
        }

    }
}