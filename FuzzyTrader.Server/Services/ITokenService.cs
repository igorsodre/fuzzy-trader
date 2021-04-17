using FuzzyTrader.Server.Data.DbEntities;

namespace FuzzyTrader.Server.Services
{
    public interface ITokenService
    {
        public string CreateAccessToken(AppUser user);

        public string CreateRefreshToken(AppUser user);
    }
}