using AutoMapper;
using FuzzyTrader.Contracts.Responses.Investment;
using FuzzyTrader.Server.Domain;

namespace FuzzyTrader.Server.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<InvestmentOptions, GetInvestmentOptionsResponse>();
        }
    }
}
