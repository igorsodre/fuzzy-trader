using AutoMapper;
using FuzzyTrader.Contracts.Requests.Investment;
using FuzzyTrader.Server.Domain;

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
