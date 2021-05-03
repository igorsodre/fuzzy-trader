namespace FuzzyTrader.Server.Domain.Investment
{
    public class InvestmentOptions
    {
        public string ProductId { get; set; }
        public string Description { get; set; }
        public decimal? BaseValue { get; set; }
        public decimal? TotalValue { get; set; }
        public string DailyTradedVolume { get; set; }
        public int Quantity { get; set; }

        public bool IsCrypto { get; set; }
    }
}
