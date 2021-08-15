using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.DTO.ExternalData
{
    public class LastStockQuote
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }       
        [JsonPropertyName("changesPercentage")]
        public decimal ChangesPercentage { get; set; }
        [JsonPropertyName("change")]
        public decimal Change { get; set; }
        [JsonPropertyName("dayLow")]
        public decimal DayLow { get; set; }
        [JsonPropertyName("dayHigh")]
        public decimal DayHigh { get; set; }
        [JsonPropertyName("yearHigh")]
        public decimal YearHigh { get; set; }
        [JsonPropertyName("yearLow")]
        public decimal YearLow { get; set; }
       
        [JsonPropertyName("priceAvg50")]
        public decimal PriceAvg50 { get; set; }
        [JsonPropertyName("priceAvg200")]
        public decimal PriceAvg200 { get; set; }
        [JsonPropertyName("previousClose")]

        public decimal PreviousClose { get; set; }
         
        [JsonPropertyName("eps")]
        public decimal? EPS { get; set; }       
    }
}
