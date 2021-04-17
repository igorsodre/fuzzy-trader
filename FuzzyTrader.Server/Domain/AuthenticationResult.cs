using System.Collections.Generic;

namespace FuzzyTrader.Server.Domain
{
    public class AuthenticationResult
    {
        public string Token { get; set; }

        public bool Succsess { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }
    }
}