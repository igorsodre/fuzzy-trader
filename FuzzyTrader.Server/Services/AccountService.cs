using System.Linq;
using System.Threading.Tasks;
using FuzzyTrader.Server.Data.DbEntities;
using FuzzyTrader.Server.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.Server.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountService(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }


        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser is not null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] {"Email aready taken"}
                };
            }

            var user = new AppUser()
            {
                Email = email,
                UserName = email,
                TokenVersion = 1
            };

            var createdUser = await _userManager.CreateAsync(user, password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = createdUser.Errors.Select(x => x.Description)
                };
            }

            var token = _tokenService.CreateAccessToken(user);

            return new AuthenticationResult
            {
                Succsess = true,
                Token = token,
                User = user
            };
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] {"Invalid Email/Password"}
                };
            }

            var verifiedPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!verifiedPassword)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] {"Invalid Email/Password"}
                };
            }

            var token = _tokenService.CreateAccessToken(user);

            return new AuthenticationResult
            {
                Succsess = true,
                Token = token,
                User = user
            };
        }

        public void AddRefreshTokenForUserOnResponse(AppUser appUser, HttpResponse httpResponse)
        {
            var token = _tokenService.CreateRefreshToken(appUser);
            _tokenService.SetHttpCookieForRefreshToken(token, httpResponse);
        }

        public async Task RevokeAllRefreshTokensForUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return;
            }

            user.TokenVersion += 1;
            await _userManager.UpdateAsync(user);
        }
    }
}