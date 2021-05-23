using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using FuzzyTrader.Server.Data.DbEntities;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Domain.Entities;
using FuzzyTrader.Server.Services.Iterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FuzzyTrader.Server.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _accesskey;
        private readonly SymmetricSecurityKey _refreshKey;
        private readonly CookieOptions _defaultCookieOptions;


        public TokenService(IConfiguration configuration)
        {
            _accesskey =
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(configuration.GetValue<string>("JwtAccessTokenSecret")));
            _refreshKey =
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(configuration.GetValue<string>("JwtRefreshTokenSecret")));
            _defaultCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Path = "/api/account/refresh_token",
                Expires = DateTime.UtcNow.AddDays(7),
                SameSite = SameSiteMode.None,
                Secure = false
            };
        }

        public string CreateAccessToken(DomainUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(CustomTokenClaims.Id, user.Id),
            };

            var credentials = new SigningCredentials(_accesskey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptior = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(7),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();


            var token = tokenHandler.CreateToken(tokenDescriptior);

            return tokenHandler.WriteToken(token);
        }

        public string CreateRefreshToken(DomainUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(CustomTokenClaims.Id, user.Id),
                new Claim(CustomTokenClaims.TokenVersion, user.TokenVersion.ToString())
            };

            var credentials = new SigningCredentials(_refreshKey, SecurityAlgorithms.HmacSha512);

            var tokenDescriptior = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptior);

            return tokenHandler.WriteToken(token);
        }

        public void SetHttpCookieForRefreshToken(string token, HttpResponse httpResponse)
        {
            httpResponse.Cookies.Append(CustomCookieKeys.RefreshToken, token, _defaultCookieOptions);
        }

        public void ClearHttpCookieForRefreshToken(HttpResponse httpResponse)
        {
            httpResponse.Cookies.Append(CustomCookieKeys.RefreshToken, "", _defaultCookieOptions);
        }

        public Dictionary<string, string> VerifyRefreshToken(string token)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _refreshKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ValidAlgorithms = new[] {SecurityAlgorithms.HmacSha512},
                ClockSkew = TimeSpan.Zero
            };

            var handler = new JwtSecurityTokenHandler();

            try
            {
                var principal = handler.ValidateToken(token, validationParameters, out var validToken);
                if (IsJwtTokenExpired(principal))
                {
                    return null;
                }

                return principal.Claims
                    .ToDictionary(k => k.Type, v => v.Value);
            }
            catch
            {
                return null;
            }
        }

        private static bool IsJwtTokenExpired(ClaimsPrincipal claimsPrincipal)
        {
            var unixExpDate =
                long.Parse(claimsPrincipal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expDate = DateTimeOffset.FromUnixTimeSeconds(unixExpDate).UtcDateTime;
            return expDate <= DateTime.UtcNow;
        }
    }
}