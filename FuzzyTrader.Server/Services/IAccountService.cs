using System.Threading.Tasks;
using FuzzyTrader.Contracts.Requests.Account;
using FuzzyTrader.Server.Data.DbEntities;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace FuzzyTrader.Server.Services
{
    public interface IAccountService
    {
        public Task<AuthenticationResult> RegisterAsync(string email, string password);
        public Task<AuthenticationResult> LoginAsync(string email, string password);

        public void Logout(HttpResponse httpResponse);
        public Task<AuthenticationResult> RefreshAccessTokenAsync(string refreshToken);
        public void AddRefreshTokenForUserOnResponse(DomainUser appUser, HttpResponse httpResponse);
        public Task RevokeAllRefreshTokensForUser(string userId);
    }
}