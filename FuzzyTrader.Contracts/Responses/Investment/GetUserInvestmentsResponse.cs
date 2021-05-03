namespace FuzzyTrader.Contracts.Responses.Investment
{
    public class GetUserInvestmentsResponse
    {
        public string Id { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public string AssetId { get; set; }
        public bool IsCrypto { get; set; }
    }
}
