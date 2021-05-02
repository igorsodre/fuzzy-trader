namespace FuzzyTrader.Server.Domain
{
    public class InvestmentOrder
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public bool IsCrypto { get; set; }
    }
}
