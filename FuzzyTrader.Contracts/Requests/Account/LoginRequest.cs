namespace FuzzyTrader.Contracts.Requests.Account
{
    public sealed class LoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}