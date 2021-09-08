using FluentValidation;
using FuzzyTrader.Contracts.Requests.Account;

namespace FuzzyTrader.Server.Validators.account
{
    public class RecoverPasswordRequestValidator : AbstractValidator<RecoverPasswordRequest>
    {
        public RecoverPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(x => x.ConfirmedPassword)
                .NotEmpty()
                .Equal(x => x.Password);

            RuleFor(x => x.Token)
                .NotEmpty();
        }
    }
}
