using System.Linq;
using System.Threading.Tasks;
using FuzzyTrader.Contracts.Requests.Account;
using FuzzyTrader.Contracts.Responses;
using FuzzyTrader.Contracts.Responses.Account;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyTrader.Server.Controllers
{
    [ApiController]
    [Route("api/account")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("signup")]
        [ProducesResponseType(typeof(SuccessResponse<SignupResponse>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<ActionResult> Signup(SignupRequest request)
        {
            var authResponse = await _accountService.RegisterAsync(request.Email, request.Password);
            if (!authResponse.Success)
            {
                return BadRequest(new BusinessErrorResponse {Errors = authResponse.ErrorMessages});
            }

            _accountService.AddRefreshTokenForUserOnResponse(authResponse.User, HttpContext.Response);

            return Ok(new SuccessResponse<SignupResponse>(new SignupResponse {Token = authResponse.Token}));
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

        // [HttpPost("update"), Authorize]
        // public async Task<ActionResult> Update()
        // {
        //     return Ok(new SuccessResponse<string>("test"));
        // }
    }
}
