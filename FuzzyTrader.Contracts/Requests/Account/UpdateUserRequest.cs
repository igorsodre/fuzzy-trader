namespace FuzzyTrader.Contracts.Requests.Account
{
    public sealed class UpdateUserRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmedPassword { get; set; }
        public string Name { get; set; }
    }
}