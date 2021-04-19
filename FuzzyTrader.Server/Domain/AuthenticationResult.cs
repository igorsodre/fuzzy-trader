using System.Collections.Generic;
using FuzzyTrader.Server.Domain.Entities;

namespace FuzzyTrader.Server.Domain
{
    public class AuthenticationResult
    {
        public string Token { get; set; }

        public DomainUser User { get; set; }

        public bool Success { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }
    }
}