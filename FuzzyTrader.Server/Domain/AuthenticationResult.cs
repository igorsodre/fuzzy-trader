using System.Collections.Generic;
using FuzzyTrader.Server.Data.DbEntities;

namespace FuzzyTrader.Server.Domain
{
    public class AuthenticationResult
    {
        public string Token { get; set; }

        public AppUser User { get; set; }

        public bool Succsess { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }
    }
}