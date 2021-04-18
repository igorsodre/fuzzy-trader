using System.Linq;
using System.Threading.Tasks;
using FuzzyTrader.Server.Data.DbEntities;
using FuzzyTrader.Server.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

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
                Success = true,
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
                Success = true,
                Token = token,
                User = user
            };
        }

        public void Logout(HttpResponse httpResponse)
        {
            _tokenService.ClearHttpCookieForRefreshToken(httpResponse);
        }

        public async Task<AuthenticationResult> RefreshAccessTokenAsync(string refreshToken)
        {
            var tokenAttributes = _tokenService.VerifyRefreshToken(refreshToken);
            if (tokenAttributes is null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] {"Invalid refresh token"}
                };
            }

            var userId = tokenAttributes[CustomTokenClaims.Id];
            var tokenVersion = int.Parse(tokenAttributes[CustomTokenClaims.TokenVersion]);

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null || user.TokenVersion != tokenVersion)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] {"Invalid refresh token"}
                };
            }

            var newAccessToken = _tokenService.CreateAccessToken(user);

            return new AuthenticationResult
            {
                Token = newAccessToken,
                Success = true
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