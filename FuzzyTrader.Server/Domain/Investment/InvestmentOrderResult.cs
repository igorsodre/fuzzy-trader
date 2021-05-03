namespace FuzzyTrader.Server.Domain.Investment
{
    public class InvestmentOrderResult
    {
        public bool Success { get; set; }
        public string[] Errors { get; set; }
    }
}
