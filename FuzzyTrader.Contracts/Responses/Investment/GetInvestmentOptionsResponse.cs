namespace FuzzyTrader.Contracts.Responses.Investment;

public class GetInvestmentOptionsResponse
{
    public string ProductId { get; set; }

    public string Description { get; set; }

    public decimal? BaseValue { get; set; }

    public decimal? TotalValue { get; set; }

    public decimal? DailyTradedVolume { get; set; }

    public int Quantity { get; set; }

    public bool IsCrypto { get; set; }
}
