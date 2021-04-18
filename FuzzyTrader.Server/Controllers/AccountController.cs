using System.Threading.Tasks;
using FuzzyTrader.Contracts.Requests.Account;
using FuzzyTrader.Contracts.Responses;
using FuzzyTrader.Contracts.Responses.Account;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Extensions;
using FuzzyTrader.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyTrader.Server.Controllers
{
    [ApiController]
    [Route("api/account")]
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
                return BadRequest(new BusinessErrorResponse
                {
                    Errors = authResponse.ErrorMessages
                });
            }

            _accountService.AddRefreshTokenForUserOnResponse(authResponse.User, HttpContext.Response);

            return Ok(new SuccessResponse<SignupResponse>(new SignupResponse
            {
                Token = authResponse.Token
            }));
        }

        [HttpPost("login")]
        public async Task<ActionResult<SuccessResponse<LoginResponse>>> Login(LoginRequest request)
        {
            var authResponse = await _accountService.LoginAsync(request.Email, request.Password);
            if (!authResponse.Succsess)
            {
                return BadRequest(new BusinessErrorResponse
                {
                    Errors = authResponse.ErrorMessages
                });
            }

            _accountService.AddRefreshTokenForUserOnResponse(authResponse.User, HttpContext.Response);

            return Ok(new SuccessResponse<LoginResponse>(new LoginResponse
            {
                Token = authResponse.Token
            }));
        }

        [HttpPost("logout")]
        public ActionResult Logout()
        {
            _accountService.Logout(HttpContext.Response);
            return Ok(new SuccessResponse<string>("OK"));
        }


        [HttpPost("refresh_token")]
        public async Task<ActionResult> RefreshToken()
        {
            var refreshToken = HttpContext.Request.Cookies[CustomCookieKeys.RefreshToken];
            var result = await _accountService.RefreshAccessTokenAsync(refreshToken);

            if (!result.Succsess)
            {
                return BadRequest(new BusinessErrorResponse
                {
                    Errors = result.ErrorMessages
                });
            }

            return Ok(new SuccessResponse<RefreshResponse>(new RefreshResponse {AccessToken = result.Token}));
        }

        // [HttpPost("update"), Authorize]
        // public async Task<ActionResult> Update()
        // {
        //     return Ok(new SuccessResponse<string>("test"));
        // }
    }
}