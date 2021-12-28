namespace FuzzyTrader.Contracts.Requests.Account
{
    public sealed class RecoverPasswordRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public string Token { get; set; }
    }
}
