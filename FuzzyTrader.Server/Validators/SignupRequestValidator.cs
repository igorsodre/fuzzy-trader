using FluentValidation;
using FuzzyTrader.Contracts.Requests.Account;

namespace FuzzyTrader.Server.Validators
{
    public class SignupRequestValidator : AbstractValidator<SignupRequest>
    {
        public SignupRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(2);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(x => x.ConfirmedPassword)
                .NotEmpty()
                .Equal(x => x.Password);
        }
    }
}
