using FluentValidation;
using FuzzyTrader.Contracts.Requests.Account;

namespace FuzzyTrader.Server.Validators.account
{
    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
