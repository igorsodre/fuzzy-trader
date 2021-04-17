using FuzzyTrader.Server.Data.DbEntities;
using Microsoft.AspNetCore.Http;

namespace FuzzyTrader.Server.Services
{
    public interface ITokenService
    {
        public string CreateAccessToken(AppUser user);

        public string CreateRefreshToken(AppUser user);

        public void SetHttpCookieForRefreshToken(string token, HttpResponse httpResponse);
    }
}