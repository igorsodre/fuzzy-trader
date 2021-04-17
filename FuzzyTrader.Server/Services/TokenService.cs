using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FuzzyTrader.Server.Data.DbEntities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FuzzyTrader.Server.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _accesskey;
        private readonly SymmetricSecurityKey _refreshKey;
        private const string TokenVersion = "TokenVersion";
        private const string Id = "Id";

        public TokenService(IConfiguration configuration)
        {
            _accesskey =
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(configuration.GetValue<string>("JwtAccessTokenSecret")));
            _refreshKey =
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(configuration.GetValue<string>("JwtRefreshTokenSecret")));
        }

        public string CreateAccessToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(Id, user.Id),
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

        public string CreateRefreshToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
                new Claim(TokenVersion, user.TokenVersion.ToString())
            };

            var credentials = new SigningCredentials(_refreshKey, SecurityAlgorithms.HmacSha512Signature);

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
    }
}