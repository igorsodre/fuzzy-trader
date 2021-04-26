using System.Collections.Generic;

namespace FuzzyTrader.Server.Domain
{
    public class InvestmentOptions
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public decimal BaseValue { get; set; }
        public decimal DailyTradedVolume { get; set; }
        public int Quantity { get; set; }
    }
}
