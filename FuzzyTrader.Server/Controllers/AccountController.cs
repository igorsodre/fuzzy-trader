using System.Linq;
using System.Threading.Tasks;
using FuzzyTrader.Contracts.Requests.Account;
using FuzzyTrader.Contracts.Responses;
using FuzzyTrader.Contracts.Responses.Account;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Options;
using FuzzyTrader.Server.Services.Iterfaces;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyTrader.Server.Controllers
{
    [ApiController]
    [Route("api/account")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        private readonly ServerSettings _serverSettings;

        public AccountController(IAccountService accountService, ServerSettings serverSettings)
        {
            _accountService = accountService;
            _serverSettings = serverSettings;
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

            return Ok(SuccessResponse.DefaultOkResponse());
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

            return Ok(SuccessResponse.DefaultOkResponse());
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
            return Ok(SuccessResponse.DefaultOkResponse());
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

        [HttpPost("forgot_password")]
        [ProducesResponseType(typeof(SuccessResponse<string>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var sentEmail = await _accountService.ForgotPasswordAysnc(request.Email);

            if (!sentEmail) return BadRequest(new ErrorResponse(new[] {"Invalid Email"}));

            return Ok(SuccessResponse.DefaultOkResponse());
        }

        [HttpGet("reset_password_page")]
        public IActionResult ReserPasswordPage(string token, string email)
        {
            var endpoint = _serverSettings.ClientUrl +
                           _serverSettings.ResetPasswordRoute +
                           $"?token={token}&email={email}";

            return Redirect(endpoint);
        }
    }
}
