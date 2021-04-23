using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FuzzyTrader.Server.Data.DbEntities;
using FuzzyTrader.Server.Domain.Entities;
using FuzzyTrader.Server.Options;
using FuzzyTrader.Server.Services;
using FuzzyTrader.Server.Services.Iterfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using NSubstitute;
using Xunit;

namespace FuzzyTrader.Tests.UnitTests
{
    public class AccountServiceTests
    {
        private readonly AccountService _sut;
        private readonly Mock<UserManager<AppUser>> _userManager;
        private readonly ITokenService _tokenService = Substitute.For<ITokenService>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly IEmailClientService _emailClientService = Substitute.For<IEmailClientService>();
        private readonly ServerSettings _serverSettings = new ServerSettings {BaseUrl = ""};

        public AccountServiceTests()
        {
            _userManager = TestHelpers.MockUserManager(new List<AppUser>());
            _sut = new AccountService(_userManager.Object, _tokenService, _mapper, _emailClientService,
                _serverSettings);
        }


        [Fact]
        public async Task RegisterAsync_ShouldReturAccessToken_WhenValidUserAndPassword()
        {
            // Setup
            var email = "test@email.com";
            var password = "Test123$";
            var testToken = "testToken";
            var testDomainUser = new DomainUser();

            _userManager.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(() => null);

            _userManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), password))
                .ReturnsAsync(IdentityResult.Success);

            _mapper.Map<DomainUser>(Arg.Any<AppUser>())
                .Returns(testDomainUser);

            _tokenService.CreateAccessToken(Arg.Any<DomainUser>())
                .Returns(testToken);

            // Action
            var result = await _sut.RegisterAsync(email, password);

            // Verify
            result.Success.Should()
                .BeTrue();
            result.Token.Should()
                .Be(testToken);
            result.User.Should()
                .Be(testDomainUser);
        }
    }
}
