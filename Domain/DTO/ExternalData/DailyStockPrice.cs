using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.DTO.ExternalData
{
    public class DailyStockPrice
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("historical")]
        public IEnumerable<HistoricDailyStockPrice> Historical { get; set; }
    }

    public class HistoricDailyStockPrice
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("open")]
        public decimal Open { get; set; }
        [JsonPropertyName("high")]
        public decimal High { get; set; }
        [JsonPropertyName("low")]
        public decimal Low { get; set; }
        [JsonPropertyName("close")]
        public decimal Close { get; set; }
    }
}
