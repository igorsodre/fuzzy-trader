using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FuzzyTrader.Contracts.External
{
    public class MartketStackEndOfDayResponse
    {
        public Pagination pagination { get; set; }
        public List<MarketStackData> data { get; set; }
    }

    public class MarketStackData
    {
        public double? open { get; set; }
        public double? high { get; set; }
        public double? low { get; set; }
        public double? close { get; set; }
        public double? volume { get; set; }
        public double? adj_high { get; set; }
        public double? adj_low { get; set; }
        public double? adj_close { get; set; }
        public double? adj_open { get; set; }
        public double? adj_volume { get; set; }
        public double? split_factor { get; set; }
        public string symbol { get; set; }
        public string exchange { get; set; }
        public string date { get; set; }
    }

    public class Pagination
    {
        public int limit { get; set; }
        public int offset { get; set; }
        public int count { get; set; }
        public int total { get; set; }
    }
}
