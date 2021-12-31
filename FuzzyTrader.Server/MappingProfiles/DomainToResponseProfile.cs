using AutoMapper;
using FuzzyTrader.Contracts.Objects;
using FuzzyTrader.Contracts.Responses.Investment;
using FuzzyTrader.Server.Domain.Entities;
using FuzzyTrader.Server.Domain.Investment;

namespace FuzzyTrader.Server.MappingProfiles;

public class DomainToResponseProfile : Profile
{
    public DomainToResponseProfile()
    {
        CreateMap<InvestmentOptions, GetInvestmentOptionsResponse>();
        CreateMap<DomainInvestment, GetUserInvestmentsResponse>();
        CreateMap<DomainUser, ResponseUser>();
    }
}