using System.Threading.Tasks;
using FuzzyTrader.Contracts.Requests.Account;
using FuzzyTrader.Server.Data.DbEntities;
using FuzzyTrader.Server.Domain;
using Microsoft.AspNetCore.Http;

namespace FuzzyTrader.Server.Services
{
    public interface IAccountService
    {
        public Task<AuthenticationResult> RegisterAsync(string email, string password);
        public Task<AuthenticationResult> LoginAsync(string email, string password);
        public void AddRefreshTokenForUserOnResponse(AppUser appUser, HttpResponse httpResponse);

        public Task RevokeAllRefreshTokensForUser(string userId);
    }
}