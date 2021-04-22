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
        public Task<bool> SendVerificationEmailAsync(string token, DomainUser user);
        public Task<bool> VerifyEmailAsync(string token, string email);
        public Task<AuthenticationResult> LoginAsync(string email, string password);
        public void Logout(HttpResponse httpResponse);
        public Task<bool> ForgotPasswordAysnc(string email);
        public Task<bool> SendForgotPasswordEmailAsync(string token, DomainUser user);
        public Task<AuthenticationResult> RefreshAccessTokenAsync(string refreshToken);
        public void AddRefreshTokenForUserOnResponse(DomainUser appUser, HttpResponse httpResponse);
        public Task RevokeAllRefreshTokensForUser(string userId);
    }
}
