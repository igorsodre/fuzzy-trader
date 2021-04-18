namespace FuzzyTrader.Contracts.Requests.Account
{
    public class UpdateUserRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewEmail { get; set; }
    }
}