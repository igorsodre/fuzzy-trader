using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FuzzyTrader.Server.Data.DbEntities;
using Microsoft.AspNetCore.Http;

namespace FuzzyTrader.Server.Services
{
    public interface ITokenService
    {
        public string CreateAccessToken(AppUser user);

        public string CreateRefreshToken(AppUser user);

        public void SetHttpCookieForRefreshToken(string token, HttpResponse httpResponse);
        public void ClearHttpCookieForRefreshToken(HttpResponse httpResponse);

        public Dictionary<string, string> VerifyRefreshToken(string token);
    }
}