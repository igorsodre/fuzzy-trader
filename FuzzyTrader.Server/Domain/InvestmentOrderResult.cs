namespace FuzzyTrader.Server.Domain
{
    public class InvestmentOrderResult
    {
        public bool Success { get; set; }
        public string[] Errors { get; set; }
    }
}
