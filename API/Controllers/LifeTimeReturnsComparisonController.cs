using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.LifeTimeReturnsComparisonService;
using Application.StockTotalValueService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LifeTimeReturnsComparisonController : ControllerBase
    {
        private readonly ILifeTimeReturnsComparisonService _lifeTimeReturnsComparisonService;

        public LifeTimeReturnsComparisonController(ILifeTimeReturnsComparisonService lifeTimeReturnsComparisonService)
        {
            _lifeTimeReturnsComparisonService = lifeTimeReturnsComparisonService;
        }

        [HttpGet("GetLifeTimeReturnsComparison/{userId}")]
        public async Task<IActionResult> GetLifeTimeReturnsComparison(ulong userId)
        {
            try
            {
                var lifeTimeReturns = await _lifeTimeReturnsComparisonService.GetLifeTimeComparison(userId);
                return Ok(lifeTimeReturns);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}