using System.Collections.Generic;
using FuzzyTrader.Server.Domain.Entities;

namespace FuzzyTrader.Server.Domain.Investment
{
    public class UserInvestmentsResult
    {
        public bool Success { get; set; }
        public string[] Errors { get; set; }
        public IEnumerable<DomainInvestment> Investments { get; set; }
    }
}
