using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Serilog.Ui.Web.Authorization.Filters;
using Xunit;

namespace Serilog.Ui.Web.Tests.Authorization.Filters;

[Trait("Ui-Authorization-Filters", "Web")]
public class PolicyAuthenticationFilterTest
{
    [Fact]
    public async Task It_returns_true_with_configured_policy()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        contextAccessor.HttpContext.Returns(httpContext);
        var authService = Substitute.For<IAuthorizationService>();
        authService
            .AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(), "passing")
            .Returns(AuthorizationResult.Success());
        var policyFilter = new PolicyAuthorizationFilter(contextAccessor, authService, "passing");

        // Act
        var result = await policyFilter.AuthorizeAsync();

        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task It_returns_false_with_configured_policy()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        contextAccessor.HttpContext.Returns(httpContext);
        var authService = Substitute.For<IAuthorizationService>();
        authService
            .AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(), "failing")
            .Returns(AuthorizationResult.Failed());
        var policyFilter = new PolicyAuthorizationFilter(contextAccessor, authService, "failing");

        // Act
        var result = await policyFilter.AuthorizeAsync();

        // Assert
        result.Should().BeFalse();
    }
}