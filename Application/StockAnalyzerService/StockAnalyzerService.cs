using Application.HoldingsService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.StockAnalyzerService
{
    public class StockAnalyzerService
    {
        private IHoldingsService _holdingsService;

        public StockAnalyzerService(IHoldingsService holdingsService)
        {
            _holdingsService = holdingsService;
        }

        public void GetComparisonWithSP500(ulong userId)
        {
            var allUserHoldings = _holdingsService.GetHoldingsForUser(userId);
        }
    }
}
