using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FuzzyTrader.Server.Data.DbEntities;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Domain.Entities;
using FuzzyTrader.Server.Options;
using FuzzyTrader.Server.Services.Iterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace FuzzyTrader.Server.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IEmailClientService _emailClientService;
        private readonly ServerSettings _serverSettings;

        public AccountService(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper,
            IEmailClientService emailClientService, ServerSettings serverSettings)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _emailClientService = emailClientService;
            _serverSettings = serverSettings;
        }


        public async Task<AuthenticationResult> RegisterAsync(string name, string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser is not null)
            {
                return new AuthenticationResult {ErrorMessages = new[] {"Email aready taken"}};
            }

            var user = new AppUser() {Name = name, Email = email, UserName = email, TokenVersion = 1};

            var createdUser = await _userManager.CreateAsync(user, password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult {ErrorMessages = createdUser.Errors.Select(x => x.Description)};
            }

            var domainUser = _mapper.Map<DomainUser>(user);

            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            await SendVerificationEmailAsync(confirmationToken, domainUser);

            return new AuthenticationResult {Success = true};
        }


        public async Task<bool> SendVerificationEmailAsync(string token, DomainUser user)
        {
            var encodedToken = WebUtility.UrlEncode(token);
            var encodedEmail = WebUtility.UrlEncode(user.Email);
            var endpoint =
                $"{_serverSettings.BaseUrl}/api/account/verify_email?token={encodedToken}&email={encodedEmail}";

            var messageText = $"<p><a href='{endpoint} target='_blank'>Click here</a> to confirm your email.</p>" +
                              $"<p>Please ignore this if you did not request it.</p>";

            var emailMessage = new EmailMessage
            {
                Subject = "Email Confirmation", Content = messageText, Reciever = user.Email
            };

            return await _emailClientService.SendEmailAsync(emailMessage);
        }

        public async Task<bool> VerifyEmailAsync(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var isConfirmed = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider,
                UserManager<AppUser>.ConfirmEmailTokenPurpose,
                token);

            if (!isConfirmed) return false;

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            await _userManager.UpdateSecurityStampAsync(user);
            return true;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return new AuthenticationResult {ErrorMessages = new[] {"Invalid Email/Password"}};
            }

            var verifiedPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!verifiedPassword)
            {
                return new AuthenticationResult {ErrorMessages = new[] {"Invalid Email/Password"}};
            }

            if (!user.EmailConfirmed)
            {
                return new AuthenticationResult {ErrorMessages = new[] {"Email not verified"}};
            }

            var domainUser = _mapper.Map<DomainUser>(user);
            var token = _tokenService.CreateAccessToken(domainUser);

            return new AuthenticationResult
            {
                Success = true,
                Token = token,
                User = domainUser
            };
        }

        public void Logout(HttpResponse httpResponse)
        {
            _tokenService.ClearHttpCookieForRefreshToken(httpResponse);
        }

        public async Task<bool> ForgotPasswordAysnc(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null || !user.EmailConfirmed)
            {
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var domainUser = _mapper.Map<DomainUser>(user);
            await SendForgotPasswordEmailAsync(token, domainUser);
            return true;
        }

        public async Task<bool> SendForgotPasswordEmailAsync(string token, DomainUser user)
        {
            var encodedToken = WebUtility.UrlEncode(token);
            var encodedEmail = WebUtility.UrlEncode(user.Email);
            var endpoint =
                $"{_serverSettings.BaseUrl}/api/account/reset_password?token={encodedToken}&email={encodedEmail}";

            var messageText = $"<p><a href='{endpoint} target='_blank'>Click here</a> to reset your password.</p>" +
                              $"<p>Please ignore this if you did not request it.</p>";

            var emailMessage = new EmailMessage
            {
                Subject = "Password Reset", Content = messageText, Reciever = user.Email
            };

            return await _emailClientService.SendEmailAsync(emailMessage);
        }

        public Task<bool> ResetPasswordAsync(string email, string token, string password)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AuthenticationResult> RefreshAccessTokenAsync(string refreshToken)
        {
            var tokenAttributes = _tokenService.VerifyRefreshToken(refreshToken);
            if (tokenAttributes is null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] {"Invalid refresh token"},
                };
            }

            var userId = tokenAttributes[CustomTokenClaims.Id];
            var tokenVersion = int.Parse(tokenAttributes[CustomTokenClaims.TokenVersion]);

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null || user.TokenVersion != tokenVersion)
            {
                return new AuthenticationResult {ErrorMessages = new[] {"Invalid refresh token"}};
            }

            var domainUser = _mapper.Map<DomainUser>(user);
            var newAccessToken = _tokenService.CreateAccessToken(domainUser);

            return new AuthenticationResult {Token = newAccessToken, Success = true, User = domainUser};
        }

        public void AddRefreshTokenForUserOnResponse(DomainUser appUser, HttpResponse httpResponse)
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
