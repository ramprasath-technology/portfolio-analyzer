using Domain.DTO.WebScraper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.WebScraperService
{
    public interface IWebScraperService
    {
        Task<IEnumerable<ScraperAnalysis>> ScrapCNNPage(IEnumerable<string> tickers);
    }
}
