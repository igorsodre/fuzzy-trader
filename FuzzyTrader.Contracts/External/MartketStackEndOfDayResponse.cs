using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FuzzyTrader.Contracts.External
{
    public class MartketStackEndOfDayResponse
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("data")]
        public List<Data> Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("open")]
        public double Open { get; set; }

        [JsonProperty("high")]
        public double High { get; set; }

        [JsonProperty("low")]
        public double Low { get; set; }

        [JsonProperty("close")]
        public double Close { get; set; }

        [JsonProperty("volume")]
        public double Volume { get; set; }

        [JsonProperty("adj_high")]
        public double AdjHigh { get; set; }

        [JsonProperty("adj_low")]
        public double AdjLow { get; set; }

        [JsonProperty("adj_close")]
        public double AdjClose { get; set; }

        [JsonProperty("adj_open")]
        public double AdjOpen { get; set; }

        [JsonProperty("adj_volume")]
        public double AdjVolume { get; set; }

        [JsonProperty("split_factor")]
        public double SplitFactor { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("exchange")]
        public string Exchange { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }

    public class Pagination
    {
        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
