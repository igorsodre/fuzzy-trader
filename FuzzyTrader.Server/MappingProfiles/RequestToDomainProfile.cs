using AutoMapper;
using FuzzyTrader.Contracts.Requests.Investment;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Domain.Investment;

namespace FuzzyTrader.Server.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<PlaceInvestmentRequest, InvestmentOrder>();
        }
    }
}
