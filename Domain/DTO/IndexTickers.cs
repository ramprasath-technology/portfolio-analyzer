using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class IndexTickers
    {
        public IReadOnlyList<string> allowedIndexTickers = new List<string>()
        {
            "VOO",
            "ONEQ",
            "QQQ"
        };
    }
}
