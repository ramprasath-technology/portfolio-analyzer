using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Application.ConfigService;
using Domain.DTO.WebScraper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.WebScraperService.CNNScraper
{
    public class CNNScraperService : IWebScraperService
    {
        private readonly IConfigService _configService;
        public CNNScraperService(IConfigService configService)
        {
            _configService = configService;
        }

        public async Task<IEnumerable<ScraperAnalysis>> ScrapCNNPage(IEnumerable<string> tickers)
        {
            const int waitTime = 1000;
            var scraperTasks = new List<Task<HttpResponseMessage>>();
            var analysis = new List<ScraperAnalysis>();
            int counter = 0;

            foreach (var ticker in tickers)
            {
                var dataFromWebPage = GetContent(ticker);
                scraperTasks.Add(dataFromWebPage);                
                Interlocked.Increment(ref counter);
                if (counter % 3 == 0)
                {
                    Thread.Sleep(waitTime);
                }
            }

            await Task.WhenAll(scraperTasks);

            foreach (var task in scraperTasks)
            {
                var dataFromWebPage = await task;
                var analysisData = await GetAnalystView(dataFromWebPage);
                //analysisData.Ticker = ticker;
                analysis.Add(analysisData);
            }
           
            return analysis;
        }

        private async Task<HttpResponseMessage> GetContent(string ticker)
        {
            var siteUrl = $"{_configService.GetCNNStockInfoUrl()}{ticker}";

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage request = await httpClient.GetAsync(siteUrl);

            return request;
        }

        private async Task<ScraperAnalysis> GetAnalystView(HttpResponseMessage request)
        {
            const string lastPriceClassName = "wsod_last";
            const string fiftyTwoWeekHighClassName = "val hi";
            const string fiftyTwoWeekLowClassName = "val lo";
            const string forecastId = "wsod_forecasts";
            const string tickerNodeClassName = "wsod_smallSubHeading";
            var scrapAnalysis = new ScraperAnalysis();

            var response = await request.Content.ReadAsStreamAsync();

            var parser = new HtmlParser();
            var document = parser.ParseDocument(response);

            var tickerNodes = document.GetElementsByClassName(tickerNodeClassName);
            INode tickerNode = null;
            if (tickerNodes.Length > 0)
            {
                tickerNode = tickerNodes[0];
            }
            SetTicker(scrapAnalysis, (IElement)tickerNode);

            var lastPriceNodes = document.GetElementsByClassName(lastPriceClassName);
            INode lastPriceNode = null;
            if (lastPriceNodes.Length > 0)
            {
                lastPriceNode = lastPriceNodes[0].FirstChild;
            }            
            SetCurrentPrice(scrapAnalysis, (IElement)lastPriceNode);

            var fiftyTwoWeekHighNodes = document.GetElementsByClassName(fiftyTwoWeekHighClassName);
            INode fiftyTwoWeekHighNode = null;
            if (fiftyTwoWeekHighNodes.Length > 0)
            {
                fiftyTwoWeekHighNode = fiftyTwoWeekHighNodes[0];
            }           
            SetFiftyTwoWeekHigh(scrapAnalysis, (IElement)fiftyTwoWeekHighNode);

            var fiftyTwoWeekLowNodes = document.GetElementsByClassName(fiftyTwoWeekLowClassName);
            INode fiftyTwoWeekLowNode = null;
            if (fiftyTwoWeekLowNodes.Length > 0)
            {
                fiftyTwoWeekLowNode = fiftyTwoWeekLowNodes[0];
            }
            SetFiftyTwoWeekLow(scrapAnalysis, (IElement)fiftyTwoWeekLowNode);

            var forecast = document.GetElementById(forecastId);
            SetAnalystRatings(scrapAnalysis, forecast);

            return scrapAnalysis;
        }

        private void SetTicker(ScraperAnalysis analysis, IElement ticker)
        {
            if (ticker != null)
            {
                var exchangeAndTickerName = ticker.InnerHtml;
                var unformattedTickerName = exchangeAndTickerName.Split(':');
                if (unformattedTickerName.Length >= 2)
                {
                    analysis.Ticker = unformattedTickerName[1].TrimEnd(')');
                }
            }
        }

        private void SetCurrentPrice(ScraperAnalysis analysis, IElement currentPrice)
        {
            if (currentPrice != null)
            {
                var valueText = currentPrice.InnerHtml;
                var value = 0.0m;
                if (Decimal.TryParse(valueText, out value))
                {
                    analysis.CurrentPrice = value;
                }
            }           
        }

        private void SetFiftyTwoWeekLow(ScraperAnalysis analysis, IElement fiftyTwoWeekLow)
        {
            if (fiftyTwoWeekLow != null)
            {
                var valueText = fiftyTwoWeekLow.InnerHtml;
                var value = 0.0m;
                if (Decimal.TryParse(valueText, out value))
                {
                    analysis.FiftyTwoWeekLow = value;
                    analysis.IncreaseFromFiftyTwoWeekLow = analysis.CurrentPrice - analysis.FiftyTwoWeekLow;
                    analysis.IncreaseFromFiftyTwoWeekLowPercentage = Decimal.Round(((analysis.IncreaseFromFiftyTwoWeekLow / analysis.FiftyTwoWeekLow) * 100), 2);
                }
            }          
        }

        private void SetFiftyTwoWeekHigh(ScraperAnalysis analysis, IElement fiftyTwoWeekHigh)
        {
            if (fiftyTwoWeekHigh != null)
            {
                var valueText = fiftyTwoWeekHigh.InnerHtml;
                var value = 0.0m;
                if (Decimal.TryParse(valueText, out value))
                {
                    analysis.FiftyTwoWeekHigh = value;
                    analysis.DecreaseFromFiftyTwoWeekHigh = analysis.FiftyTwoWeekHigh - analysis.CurrentPrice;
                    analysis.DecreaseFromFiftyTwoWeekHighPercentage = Decimal.Round(((analysis.DecreaseFromFiftyTwoWeekHigh / analysis.FiftyTwoWeekHigh) * 100), 2);
                }
            }        
        }

        private void SetAnalystRatings(ScraperAnalysis analysis, IElement forecast)
        {
            if (forecast != null)
            {
                var analysisText = forecast.GetElementsByTagName("p");

                for (var j = 0; j < analysisText.Length; j++)
                {
                    if (j == 0)
                    {
                        SetLowMedianHighEstimates(analysisText[0], analysis);
                    }    
                    if (j == 1)
                    {
                        SetCurrentRecommendation(analysisText[1], analysis);
                        break;
                    }
                }
            }
        }

        private void SetCurrentRecommendation(IElement analysisText, ScraperAnalysis analysis)
        {
            var regex = new Regex("\\<[^\\>]*\\>");
            var recommendation = regex.Replace(analysisText.InnerHtml, String.Empty);
            analysis.CurrentRecommendation = recommendation;
        }

        private void SetLowMedianHighEstimates(IElement analysisText, ScraperAnalysis analysis)
        {
            var text = analysisText.InnerHtml;
            var percentage = analysisText.Children.Length > 0 ? analysisText.Children[0].InnerHtml : "0";
            SetDifferenceFromMedianTarget(percentage, analysis);
            var words = text.Split(' ');
            for (var i = 0; i < words.Length; i++)
            {
                if ((i != 0) && (i != words.Length - 1))
                {
                    if (words[i - 1] == "The" && words[i + 1] == "analysts")
                    {
                        int numberOfAnalysts = 0;
                        bool isParsable = Int32.TryParse(words[i], out numberOfAnalysts);
                        if (isParsable)
                        {
                            analysis.NumberOfAnalysts = numberOfAnalysts;
                        }
                        i++;
                    }

                    if (words[i] == "median" && words[i + 1] == "target" && words[i + 2] == "of")
                    {
                        decimal medianTarget = 0m;
                        bool isParsable = Decimal.TryParse(words[i + 3].TrimEnd(','), out medianTarget);
                        if (isParsable)
                        {
                            analysis.MedianTarget = medianTarget;
                        }
                        i = i + 3;
                    }

                    if (words[i] == "high" && words[i + 1] == "estimate" && words[i + 2] == "of")
                    {
                        decimal highEstimate = 0m;
                        bool isParsable = Decimal.TryParse(words[i + 3], out highEstimate);
                        if (isParsable)
                        {
                            analysis.HighTarget = highEstimate;
                        }
                        i = i + 3;
                    }

                    if (words[i] == "low" && words[i + 1] == "estimate" && words[i + 2] == "of")
                    {
                        decimal lowEstimate = 0m;
                        bool isParsable = Decimal.TryParse(words[i + 3].TrimEnd('.'), out lowEstimate);
                        if (isParsable)
                        {
                            analysis.LowTarget = lowEstimate;
                        }
                        i = i + 3;
                    }
                }
            }
        }
        
        private void SetDifferenceFromMedianTarget(string difference, ScraperAnalysis analysis)
        {
            var positive = true;
            decimal percentageChange = 0m;
            var trimChars = new char[3] { '+', '-', '%' };
            if (difference.IndexOf('-') == 0)
            {
                positive = false;
            }
            difference = difference.Trim(trimChars);
            bool isParsable = Decimal.TryParse(difference, out percentageChange);
            if (isParsable)
            {
                if (positive)
                {
                    analysis.DifferenceFromMedianPercentage = percentageChange;
                }
                else
                {
                    analysis.DifferenceFromMedianPercentage = percentageChange * (-1);
                }
            }
        }

    }
}
