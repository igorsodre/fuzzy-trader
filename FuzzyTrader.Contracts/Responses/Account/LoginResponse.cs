using FuzzyTrader.Contracts.Objects;

namespace FuzzyTrader.Contracts.Responses.Account
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public ResponseUser User { get; set; }
    }
}