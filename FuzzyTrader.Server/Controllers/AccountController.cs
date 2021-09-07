using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FuzzyTrader.Contracts.Objects;
using FuzzyTrader.Contracts.Requests.Account;
using FuzzyTrader.Contracts.Responses;
using FuzzyTrader.Contracts.Responses.Account;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Options;
using FuzzyTrader.Server.Services.Iterfaces;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyTrader.Server.Controllers
{
    [Route("api/account")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        private readonly ServerSettings _serverSettings;

        public AccountController(IAccountService accountService, ServerSettings serverSettings, IMapper mapper)
        {
            _accountService = accountService;
            _serverSettings = serverSettings;
            _mapper = mapper;
        }

        [HttpPost("signup")]
        [ProducesResponseType(typeof(SuccessResponse<string>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<ActionResult> Signup(SignupRequest request)
        {
            var authResponse = await _accountService.RegisterAsync(request.Name, request.Email, request.Password);
            if (!authResponse.Success)
            {
                return BadRequest(new BusinessErrorResponse { Errors = authResponse.ErrorMessages });
            }

            return Ok(SuccessResponse.DefaultOkResponse());
        }

        [HttpGet("verify-email")]
        [ProducesResponseType(typeof(SuccessResponse<string>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<ActionResult> VerifyEmail(string token, string email)
        {
            var isVerified = await _accountService.VerifyEmailAsync(token, email);
            if (!isVerified)
            {
                return BadRequest(new ErrorResponse(new[] { "Failed to verify email" }));
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
                return BadRequest(new ErrorResponse(authResponse.ErrorMessages));
            }

            _accountService.AddRefreshTokenForUserOnResponse(authResponse.User, HttpContext.Response);

            var responseUser = _mapper.Map<ResponseUser>(authResponse.User);

            return Ok(new SuccessResponse<LoginResponse>(new LoginResponse
                { AccessToken = authResponse.Token, User = responseUser }));
        }

        [HttpPost("logout")]
        [ProducesResponseType(typeof(SuccessResponse<string>), 200)]
        public IActionResult Logout()
        {
            _accountService.Logout(HttpContext.Response);
            return Ok(SuccessResponse.DefaultOkResponse());
        }


        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(SuccessResponse<RefreshTokenResponse>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = HttpContext.Request.Cookies[CustomCookieKeys.RefreshToken];
            var result = await _accountService.RefreshAccessTokenAsync(refreshToken);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponse(result.ErrorMessages));
            }

            var user = _mapper.Map<ResponseUser>(result.User);
            return Ok(new SuccessResponse<RefreshTokenResponse>(new RefreshTokenResponse
                { AccessToken = result.Token, User = user }));
        }

        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(SuccessResponse<string>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var sentEmail = await _accountService.ForgotPasswordAysnc(request.Email);

            if (!sentEmail) return BadRequest(new ErrorResponse(new[] { "Invalid Email" }));

            return Ok(SuccessResponse.DefaultOkResponse());
        }

        [HttpGet("reset-password-page")]
        public IActionResult ReserPasswordPage(string token, string email)
        {
            var endpoint = _serverSettings.ClientUrl +
                           _serverSettings.ResetPasswordRoute +
                           $"?token={token}&email={email}";

            return Redirect(endpoint);
        }
    }
}
