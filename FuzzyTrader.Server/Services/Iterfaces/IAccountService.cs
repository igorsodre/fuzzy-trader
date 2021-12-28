using System.Threading.Tasks;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace FuzzyTrader.Server.Services.Iterfaces;

public interface IAccountService
{
    public Task<AuthenticationResult> RegisterAsync(string name, string email, string password);
    public Task<AuthenticationResult> LoginAsync(string email, string password);
    public void Logout(HttpResponse httpResponse);

    public Task<DefaultResult> UpdateUserAsync(string userId, string password, string name, string newPassword);
    public Task<AuthenticationResult> RefreshAccessTokenAsync(string refreshToken);
    public Task<DefaultResult> VerifyEmailAsync(string token, string email);
    public Task<DefaultResult> ForgotPasswordAysnc(string email);
    public Task<DefaultResult> RecoverPassword(string email, string password, string token);
    public void AddRefreshTokenForUserOnResponse(DomainUser appUser, HttpResponse httpResponse);
}