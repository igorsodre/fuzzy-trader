namespace FuzzyTrader.Server.Domain.Entities
{
    public class DomainInvestment
    {
        public string Id { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public string AssetId { get; set; }
        public bool IsCrypto { get; set; }
    }
}
