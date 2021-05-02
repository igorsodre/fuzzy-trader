using FluentValidation;
using FuzzyTrader.Contracts.Requests.Investment;

namespace FuzzyTrader.Server.Validators.Investment
{
    public class PlaceInvestmentRequestValidatior : AbstractValidator<PlaceInvestmentRequest>
    {
        public PlaceInvestmentRequestValidatior()
        {
            RuleFor(x => x.Quantity)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.IsCrypto)
                .NotNull();

            RuleFor(x => x.ProductId)
                .NotNull()
                .NotEmpty();
        }
    }
}
