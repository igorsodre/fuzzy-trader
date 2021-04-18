using System.Threading.Tasks;
using FuzzyTrader.Contracts.Requests.Account;
using FuzzyTrader.Contracts.Responses;
using FuzzyTrader.Contracts.Responses.Account;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Extensions;
using FuzzyTrader.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyTrader.Server.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("signup")]
        public async Task<ActionResult> Signup(SignupRequest request)
        {
            var authResponse = await _accountService.RegisterAsync(request.Email, request.Password);
            if (!authResponse.Succsess)
            {
                return BadRequest(new ErrorResponse
                {
                    Errors = authResponse.ErrorMessages
                });
            }

            _accountService.AddRefreshTokenForUserOnResponse(authResponse.User, HttpContext.Response);

            return Ok(new Response<SignupResponse>(new SignupResponse
            {
                Token = authResponse.Token
            }));
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<LoginResponse>>> Login(LoginRequest request)
        {
            var authResponse = await _accountService.LoginAsync(request.Email, request.Password);
            if (!authResponse.Succsess)
            {
                return BadRequest(new ErrorResponse
                {
                    Errors = authResponse.ErrorMessages
                });
            }

            _accountService.AddRefreshTokenForUserOnResponse(authResponse.User, HttpContext.Response);

            return Ok(new Response<LoginResponse>(new LoginResponse
            {
                Token = authResponse.Token
            }));
        }

        [HttpPost("logout")]
        public ActionResult Logout()
        {
            _accountService.Logout(HttpContext.Response);
            return Ok(new Response<string>("OK"));
        }


        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh()
        {
            var refreshToken = HttpContext.Request.Cookies[CustomCookieKeys.RefreshToken];
            var result = await _accountService.RefreshAccessTokenAsync(refreshToken);

            if (!result.Succsess)
            {
                return BadRequest(new ErrorResponse
                {
                    Errors = result.ErrorMessages
                });
            }

            return Ok(new Response<RefreshResponse>(new RefreshResponse {AccessToken = result.Token}));
        }
    }
}