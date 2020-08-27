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

        [HttpGet("GetCompositionAndRecommendation/{userId}")]
        public async Task<IActionResult> GetCompositionAndRecommendation(ulong userId)
        {
            var compositionAndRecommendation = await _compositionAndRecommendationService.GetCompositionAndRecommendation(userId);
            return Ok(compositionAndRecommendation);
        }

    }
}