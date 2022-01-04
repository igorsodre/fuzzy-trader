using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FuzzyTrader.DataAccess.Entities;
using FuzzyTrader.DataAccess.Interfaces;
using FuzzyTrader.Server.Domain.Entities;
using FuzzyTrader.Server.Options;
using FuzzyTrader.Server.Services;
using FuzzyTrader.Server.Services.Iterfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using NSubstitute;
using Xunit;

namespace FuzzyTrader.Tests.UnitTests;

public class AccountServiceTests
{
    private readonly AccountService _sut;
    private readonly IAccountManager _accountManager = Substitute.For<IAccountManager>();
    private readonly ITokenService _tokenService = Substitute.For<ITokenService>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IEmailClientService _emailClientService = Substitute.For<IEmailClientService>();
    private readonly ServerSettings _serverSettings = new() { BaseUrl = "" };

    public AccountServiceTests()
    {
        _sut = new AccountService(_accountManager, _tokenService, _mapper, _emailClientService, _serverSettings);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturAccessToken_WhenValidUserAndPassword()
    {
        // Setup
        var name = "test name";
        var email = "test@email.com";
        var password = "Test123$";
        var testToken = "testToken";
        var testDomainUser = new DomainUser();

        _accountManager.FindByEmailAsync(email).Returns(Task.FromResult(default(AppUser)));

        _accountManager.CreateAsync(Arg.Any<AppUser>(), password).Returns(Task.FromResult(IdentityResult.Success));

        _mapper.Map<DomainUser>(Arg.Any<AppUser>()).Returns(testDomainUser);

        _tokenService.CreateAccessToken(Arg.Any<DomainUser>()).Returns(testToken);

        // Action
        var result = await _sut.RegisterAsync(name, email, password);

        // Verify
        result.Success.Should().BeTrue();
        result.Token.Should().Be(testToken);
        result.User.Should().Be(testDomainUser);
    }
}
