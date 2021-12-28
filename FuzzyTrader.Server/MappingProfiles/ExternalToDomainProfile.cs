using AutoMapper;
using CoinAPI.REST.V1;
using FuzzyTrader.Server.Domain.Entities;

namespace FuzzyTrader.Server.MappingProfiles;

public class ExternalToDomainProfile : Profile
{
    public ExternalToDomainProfile()
    {
        CreateMap<Asset, DomainInvestment>()
            .ForMember(x => x.Description, opt => opt.MapFrom(asset => asset.name))
            .ForMember(x => x.Value, opt => opt.MapFrom(asset => asset.price_usd))
            .ForMember(x => x.AssetId, opt => opt.MapFrom(asset => asset.asset_id))
            .ForMember(x => x.IsCrypto, opt => opt.MapFrom(asset => asset.type_is_crypto));
    }
}