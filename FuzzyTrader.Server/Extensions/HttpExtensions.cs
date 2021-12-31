using System.Linq;
using FuzzyTrader.Server.Domain;
using Microsoft.AspNetCore.Http;

namespace FuzzyTrader.Server.Extensions;

public static class HttpExtensions
{
    public static string GetUserId(this HttpContext httpContext)
    {
        return httpContext.User.Claims.SingleOrDefault(x => x.Type == CustomTokenClaims.Id)?.Value ?? string.Empty;
    }
}