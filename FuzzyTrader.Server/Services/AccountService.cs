using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FuzzyTrader.DataAccess.Entities;
using FuzzyTrader.DataAccess.Interfaces;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Domain.Entities;
using FuzzyTrader.Server.Options;
using FuzzyTrader.Server.Services.Iterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace FuzzyTrader.Server.Services;

public class AccountService : IAccountService
{
    private readonly IAccountManager _accountManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IEmailClientService _emailClientService;
    private readonly ServerSettings _serverSettings;

    public AccountService(
        IAccountManager accountManager,
        ITokenService tokenService,
        IMapper mapper,
        IEmailClientService emailClientService,
        ServerSettings serverSettings
    )
    {
        _accountManager = accountManager;
        _tokenService = tokenService;
        _mapper = mapper;
        _emailClientService = emailClientService;
        _serverSettings = serverSettings;
    }

    public async Task<AuthenticationResult> RegisterAsync(string name, string email, string password)
    {
        var existingUser = await _accountManager.FindByEmailAsync(email);

        if (existingUser is not null)
        {
            return new AuthenticationResult { ErrorMessages = new[] { "Email aready taken" } };
        }

        var user = new AppUser()
        {
            Name = name,
            Email = email,
            UserName = email,
            TokenVersion = 1
        };

        var createdUser = await _accountManager.CreateAsync(user, password);

        if (!createdUser.Succeeded)
        {
            return new AuthenticationResult { ErrorMessages = createdUser.Errors.Select(x => x.Description).ToList() };
        }

        var domainUser = _mapper.Map<DomainUser>(user);

        var confirmationToken = await _accountManager.GenerateEmailConfirmationTokenAsync(user);

        await SendVerificationEmailAsync(confirmationToken, domainUser);

        return new AuthenticationResult { Success = true };
    }

    public async Task<DefaultResult> SendVerificationEmailAsync(string token, DomainUser user)
    {
        var encodedToken = WebUtility.UrlEncode(token);
        var encodedEmail = WebUtility.UrlEncode(user.Email);
        var endpoint = $"{_serverSettings.BaseUrl}/api/account/verify-email?token={encodedToken}&email={encodedEmail}";

        var messageText = $"<p><a href='{endpoint}' target='_blank'>Click here</a> to confirm your email.</p>" +
                          $"<p>Please ignore this if you did not request it.</p>";

        var emailMessage = new EmailMessage
        {
            Subject = "Email Confirmation",
            Content = messageText,
            Reciever = user.Email
        };

        return await _emailClientService.SendEmailAsync(emailMessage);
    }

    public async Task<DefaultResult> VerifyEmailAsync(string token, string email)
    {
        var user = await _accountManager.FindByEmailAsync(email);
        var isConfirmed = await _accountManager.VerifyUserTokenAsync(
            user,
            TokenOptions.DefaultProvider,
            UserManager<AppUser>.ConfirmEmailTokenPurpose,
            token
        );

        if (!isConfirmed)
        {
            return new DefaultResult
            {
                Success = false,
                Errors = new[] { "Failed to verify email" }
            };
        }

        user.EmailConfirmed = true;
        await _accountManager.UpdateAsync(user);
        await _accountManager.UpdateSecurityStampAsync(user);
        return new DefaultResult { Success = true };
    }

    public async Task<AuthenticationResult> LoginAsync(string email, string password)
    {
        var user = await _accountManager.FindByEmailAsync(email);

        if (user is null)
        {
            return new AuthenticationResult { ErrorMessages = new[] { "Invalid Email/Password" } };
        }

        var verifiedPassword = await _accountManager.CheckPasswordAsync(user, password);

        if (!verifiedPassword)
        {
            return new AuthenticationResult { ErrorMessages = new[] { "Invalid Email/Password" } };
        }

        if (!user.EmailConfirmed)
        {
            return new AuthenticationResult { ErrorMessages = new[] { "Email not verified" } };
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

    public async Task<DefaultResult> UpdateUserAsync(string userId, string password, string name, string newPassword)
    {
        var user = await _accountManager.FindByIdAsync(userId);

        var verifiedPassword = await _accountManager.CheckPasswordAsync(user, password);
        if (!verifiedPassword)
        {
            return new DefaultResult() { Errors = new[] { "Invalid Email/Password" } };
        }

        if (!user.EmailConfirmed)
        {
            return new DefaultResult { Errors = new[] { "Email not verified" } };
        }

        user.Name = name;
        var result = await _accountManager.ChangePasswordAsync(user, password, newPassword);
        if (!result.Succeeded)
        {
            return new DefaultResult { Errors = result.Errors.Select(x => x.Description).ToList() };
        }

        return new DefaultResult { Success = true };
    }

    public async Task<DefaultResult> ForgotPasswordAysnc(string email)
    {
        var user = await _accountManager.FindByEmailAsync(email);

        if (user is null || !user.EmailConfirmed)
        {
            return new DefaultResult { Errors = new[] { "Invalid Email" } };
        }

        var token = await _accountManager.GeneratePasswordResetTokenAsync(user);
        var domainUser = _mapper.Map<DomainUser>(user);

        return await SendForgotPasswordEmailAsync(token, domainUser);
    }

    public async Task<DefaultResult> RecoverPassword(string email, string password, string token)
    {
        var user = await _accountManager.FindByEmailAsync(email);

        if (user is null || !user.EmailConfirmed)
        {
            return new DefaultResult { Errors = new[] { "Invalid Email" } };
        }

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

        var result = await _accountManager.ResetPasswordAsync(user, decodedToken, password);
        if (!result.Succeeded)
        {
            return new DefaultResult
            {
                Success = false,
                Errors = result.Errors.Select(error => error.Description).ToList()
            };
        }

        await _accountManager.UpdateSecurityStampAsync(user);
        await RevokeAllRefreshTokensForUser(user.Id);

        return new DefaultResult { Success = true };
    }

    public async Task<AuthenticationResult> RefreshAccessTokenAsync(string refreshToken)
    {
        var tokenAttributes = _tokenService.VerifyRefreshToken(refreshToken);
        if (tokenAttributes is null)
        {
            return new AuthenticationResult
            {
                ErrorMessages = new[] { "Invalid refresh token" },
            };
        }

        var userId = tokenAttributes[CustomTokenClaims.Id];
        var tokenVersion = int.Parse(tokenAttributes[CustomTokenClaims.TokenVersion]);

        var user = await _accountManager.FindByIdAsync(userId);

        if (user is null || user.TokenVersion != tokenVersion)
        {
            return new AuthenticationResult { ErrorMessages = new[] { "Invalid refresh token" } };
        }

        var domainUser = _mapper.Map<DomainUser>(user);
        var newAccessToken = _tokenService.CreateAccessToken(domainUser);

        return new AuthenticationResult
        {
            Token = newAccessToken,
            Success = true,
            User = domainUser
        };
    }

    public void AddRefreshTokenForUserOnResponse(DomainUser appUser, HttpResponse httpResponse)
    {
        var token = _tokenService.CreateRefreshToken(appUser);
        _tokenService.SetHttpCookieForRefreshToken(token, httpResponse);
    }

    private async Task<DefaultResult> SendForgotPasswordEmailAsync(string token, DomainUser user)
    {
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var encodedEmail = WebUtility.UrlEncode(user.Email);
        var endpoint =
            $"{_serverSettings.BaseUrl}/api/account/reset-password?token={encodedToken}&email={encodedEmail}";

        var messageText = $"<p><a href='{endpoint}' target='_blank'>Click here</a> to reset your password.</p>" +
                          $"<p>Please ignore this if you did not request it.</p>";

        var emailMessage = new EmailMessage
        {
            Subject = "Password Reset",
            Content = messageText,
            Reciever = user.Email
        };

        return await _emailClientService.SendEmailAsync(emailMessage);
    }

    private async Task RevokeAllRefreshTokensForUser(string userId)
    {
        var user = await _accountManager.FindByIdAsync(userId);
        if (user is null)
        {
            return;
        }

        user.TokenVersion += 1;
        await _accountManager.UpdateAsync(user);
    }
}
