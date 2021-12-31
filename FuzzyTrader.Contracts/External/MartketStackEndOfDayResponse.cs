using System.Collections.Generic;

namespace FuzzyTrader.Contracts.External;

public class MartketStackEndOfDayResponse
{
    public Pagination pagination { get; set; }
    public List<MarketStackData> data { get; set; }
}

public class MarketStackData
{
    public decimal? open { get; set; }
    public decimal? high { get; set; }
    public decimal? low { get; set; }
    public decimal? close { get; set; }
    public decimal? volume { get; set; }
    public decimal? adj_high { get; set; }
    public decimal? adj_low { get; set; }
    public decimal? adj_close { get; set; }
    public decimal? adj_open { get; set; }
    public decimal? adj_volume { get; set; }
    public decimal? split_factor { get; set; }
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