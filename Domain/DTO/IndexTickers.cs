using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public static class IndexTickers
    {
        private static IReadOnlyList<string> allowedIndexTickers = new List<string>()
        {
            "VOO",
            "ONEQ",
            //"QQQ",
            //"ARKQ",
            //"ARKW",
            //"ARKG",
            //"ARKF"
        };

        public static IReadOnlyList<string> GetAllowedIndexTickers()
        {
            return allowedIndexTickers;
        }
    }
}
