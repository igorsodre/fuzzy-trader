namespace FuzzyTrader.Contracts.Requests.Account
{
    public sealed class SignupRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
    }
}
