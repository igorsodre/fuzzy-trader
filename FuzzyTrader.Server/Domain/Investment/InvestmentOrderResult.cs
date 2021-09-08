using System.Collections.Generic;

namespace FuzzyTrader.Server.Domain.Investment
{
    public class InvestmentOrderResult
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
