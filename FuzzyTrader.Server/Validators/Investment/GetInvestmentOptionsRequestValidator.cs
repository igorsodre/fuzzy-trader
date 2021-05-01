using FluentValidation;
using FuzzyTrader.Contracts.Requests.Investment;

namespace FuzzyTrader.Server.Validators.Investment
{
    public class GetInvestmentOptionsRequestValidator : AbstractValidator<GetInvestmentOptionsRequest>
    {
        public GetInvestmentOptionsRequestValidator()
        {
            RuleFor(x => x.Budget)
                .NotNull()
                .GreaterThan(0);
        }
    }
}
