using AutoMapper;
using FuzzyTrader.DataAccess.Entities;
using FuzzyTrader.Server.Domain.Entities;

namespace FuzzyTrader.Server.MappingProfiles;

public class DatabaseToDomainProfile : Profile
{
    public DatabaseToDomainProfile()
    {
        CreateMap<AppUser, DomainUser>();
    }
}