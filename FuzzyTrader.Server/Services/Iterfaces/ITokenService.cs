using System.Collections.Generic;
using FuzzyTrader.Server.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace FuzzyTrader.Server.Services.Iterfaces;

public interface ITokenService
{
    public string CreateAccessToken(DomainUser user);

    public string CreateRefreshToken(DomainUser user);

    public void SetHttpCookieForRefreshToken(string token, HttpResponse httpResponse);
    public void ClearHttpCookieForRefreshToken(HttpResponse httpResponse);

    public Dictionary<string, string> VerifyRefreshToken(string token);
}