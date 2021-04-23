using FluentValidation;
using FuzzyTrader.Contracts.Requests.Account;

namespace FuzzyTrader.Server.Validators
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Token)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(6)
                .Equal(y => y.ConfirmedPassword);
        }
    }
}
