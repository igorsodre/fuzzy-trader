using System.Threading.Tasks;
using FuzzyTrader.Server.Data;
using FuzzyTrader.Server.Data.DbEntities;
using FuzzyTrader.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.Server.Controllers
{
    [ApiController]
    public class Test : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;

        public Test(DataContext dataContext, ITokenService tokenService, UserManager<AppUser> userManager)
        {
            _dataContext = dataContext;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet("/test")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _dataContext.Users.ToListAsync());
        }

        [HttpGet("/test2")]
        public async Task<IActionResult> Get2()
        {
            return Ok(_tokenService.CreateAccessToken(await _userManager.Users.FirstAsync()));
        }
    }
}