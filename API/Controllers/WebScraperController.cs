using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.WebScraperService;
using Domain.DTO.WebScraper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebScraperController : ControllerBase
    {
        private readonly IWebScraperService _webScraperService;

        public WebScraperController(IWebScraperService webScraperService)
        {
            _webScraperService = webScraperService;
        }

        [HttpGet("GetAnalystView/{ticker}")]
        public async Task<ScraperAnalysis> GetAnalystView(string ticker)
        {
            /*var analysis = await _webScraperService.GetAnalystView(ticker);
            return analysis;*/

            return null;
        }
    }
}