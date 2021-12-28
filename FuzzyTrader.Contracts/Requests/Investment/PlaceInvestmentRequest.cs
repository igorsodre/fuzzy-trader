namespace FuzzyTrader.Contracts.Requests.Investment;

public sealed class PlaceInvestmentRequest
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public bool IsCrypto { get; set; }
}