using AutoMapper;
using CoinAPI.REST.V1;
using FuzzyTrader.Server.Data.DbEntities;

namespace FuzzyTrader.Server.MappingProfiles
{
    public class ExternalToDatabaseProfile : Profile
    {
        public ExternalToDatabaseProfile()
        {
            CreateMap<Asset, CryptoCoin>()
                .ForMember(x => x.Name, opt => opt.MapFrom(asset => asset.name))
                .ForMember(x => x.AssetId, opt => opt.MapFrom(asset => asset.asset_id))
                .ForMember(x => x.PriceUsd, opt => opt.MapFrom(asset => asset.price_usd))
                .ForMember(x => x.DataOrderbookEnd, opt => opt.MapFrom(asset => asset.data_orderbook_end))
                .ForMember(x => x.DataOrderbookStart, opt => opt.MapFrom(asset => asset.data_orderbook_start))
                .ForMember(x => x.DataQuoteCount, opt => opt.MapFrom(asset => asset.data_quote_count))
                .ForMember(x => x.DataQuoteEnd, opt => opt.MapFrom(asset => asset.data_quote_end))
                .ForMember(x => x.DataQuoteStart, opt => opt.MapFrom(asset => asset.data_quote_start))
                .ForMember(x => x.DataSymbolsCount, opt => opt.MapFrom(asset => asset.data_symbols_count))
                .ForMember(x => x.DataTradeCount, opt => opt.MapFrom(asset => asset.data_trade_count))
                .ForMember(x => x.DataTradeEnd, opt => opt.MapFrom(asset => asset.data_trade_end))
                .ForMember(x => x.DataTradeStart, opt => opt.MapFrom(asset => asset.data_trade_start))
                .ForMember(x => x.TypeIsCrypto, opt => opt.MapFrom(asset => asset.type_is_crypto))
                .ForMember(x => x.Volume1DayUsd, opt => opt.MapFrom(asset => asset.volume_1day_usd))
                .ForMember(x => x.Volume1HrsUsd, opt => opt.MapFrom(asset => asset.volume_1hrs_usd))
                .ForMember(x => x.Volume1MthUsd, opt => opt.MapFrom(asset => asset.volume_1mth_usd));
        }
    }
}
