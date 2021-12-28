using AutoMapper;
using FuzzyTrader.Server.Data.DbEntities;
using FuzzyTrader.Server.Domain.Entities;

namespace FuzzyTrader.Server.MappingProfiles;

public class DatabaseToDomainProfile : Profile
{
    public DatabaseToDomainProfile()
    {
        CreateMap<AppUser, DomainUser>();
    }
}