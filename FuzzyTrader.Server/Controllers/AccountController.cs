using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using FuzzyTrader.Contracts.Requests.Account;
using FuzzyTrader.Contracts.Responses;
using FuzzyTrader.Contracts.Responses.Account;
using FuzzyTrader.Server.Data;
using FuzzyTrader.Server.Data.DbEntities;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.Server.Controllers
{
    [ApiController]
    [Route("api/account")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _dataContext;

        public AccountController(IAccountService accountService, UserManager<AppUser> userManager,
            DataContext dataContext)
        {
            _accountService = accountService;
            _userManager = userManager;
            _dataContext = dataContext;
        }

        [HttpPost("signup")]
        [ProducesResponseType(typeof(SuccessResponse<string>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<ActionResult> Signup(SignupRequest request)
        {
            var authResponse = await _accountService.RegisterAsync(request.Email, request.Password);
            if (!authResponse.Success)
            {
                return BadRequest(new BusinessErrorResponse {Errors = authResponse.ErrorMessages});
            }

            return Ok(new SuccessResponse<string>("OK"));
        }

        [HttpGet("verify_email")]
        [ProducesResponseType(typeof(SuccessResponse<string>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<ActionResult> VerifyEmail(string token, string email)
        {
            var isVerified = await _accountService.VerifyEmailAsync(token, email);
            if (!isVerified)
            {
                return BadRequest(new ErrorResponse(new[] {"Failed to verify email"}));
            }

            return Ok(new SuccessResponse<string>("OK"));
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(SuccessResponse<LoginResponse>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var authResponse = await _accountService.LoginAsync(request.Email, request.Password);
            if (!authResponse.Success)
            {
                return BadRequest(new ErrorResponse(authResponse.ErrorMessages.ToArray()));
            }

            _accountService.AddRefreshTokenForUserOnResponse(authResponse.User, HttpContext.Response);

            return Ok(new SuccessResponse<LoginResponse>(new LoginResponse {Token = authResponse.Token}));
        }

        [HttpPost("logout")]
        [ProducesResponseType(typeof(SuccessResponse<string>), 200)]
        public IActionResult Logout()
        {
            _accountService.Logout(HttpContext.Response);
            return Ok(new SuccessResponse<string>("OK"));
        }


        [HttpPost("refresh_token")]
        [ProducesResponseType(typeof(SuccessResponse<RefreshResponse>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = HttpContext.Request.Cookies[CustomCookieKeys.RefreshToken];
            var result = await _accountService.RefreshAccessTokenAsync(refreshToken);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponse(result.ErrorMessages.ToArray()));
            }

            return Ok(new SuccessResponse<RefreshResponse>(new RefreshResponse {AccessToken = result.Token}));
        }

        [HttpPost("remove_all_users")]
        public async Task<ActionResult> RemoveAllUsers()
        {
            await using var trasaction = await _dataContext.Database.BeginTransactionAsync();

            var users = await _dataContext.Users.ToListAsync();

            foreach (var user in users)
            {
                await _userManager.DeleteAsync(user);
            }


            await trasaction.CommitAsync();

            return Ok(new SuccessResponse<string>("test"));
        }
    }
}
