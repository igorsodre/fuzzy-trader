namespace FuzzyTrader.Contracts.Requests.Investment
{
    public class PlaceInvestmentRequest
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public bool IsCrypto { get; set; }
    }
}
