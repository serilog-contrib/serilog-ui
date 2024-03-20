using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using NSubstitute;
using Serilog.Ui.Web.Authorization.Filters;
using Xunit;

namespace Serilog.Ui.Web.Tests.Authorization;

[Trait("Ui-Authorization", "Web")]
public class BasicAuthenticationFilterTests
{
    [Fact]
    public void Authorize_WithValidCredentials_ShouldReturnTrue()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Authorization = "Basic VXNlcjpQQHNz"; // Base64 encoded "User:P@ss"
        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        contextAccessor.HttpContext.Returns(httpContext);

        var filter = new BasicAuthenticationFilter(contextAccessor)
        {
            UserName = "User",
            Password = "P@ss"
        };

        // Act
        var result = filter.Authorize();
        var authCookie = httpContext.Response.GetTypedHeaders().SetCookie
            .FirstOrDefault(sc => sc.Name == BasicAuthenticationFilter.AuthenticationCookieName);

        // Assert
        result.Should().BeTrue();
        authCookie.Should().NotBeNull();
    }

    [Fact]
    public void Authorize_WithInvalidCredentials_ShouldReturnFalse()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Authorization = "Basic QWRtaW46dXNlcg=="; // Base64 encoded "Admin:user"
        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        contextAccessor.HttpContext.Returns(httpContext);
        var filter = new BasicAuthenticationFilter(contextAccessor)
        {
            UserName = "User",
            Password = "P@ss"
        };

        // Act
        var result = filter.Authorize();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Authorize_WithMissingAuthorizationHeader_ShouldSetChallengeResponse()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        contextAccessor.HttpContext.Returns(httpContext);
        var filter = new BasicAuthenticationFilter(contextAccessor)
        {
            UserName = "User",
            Password = "P@ss"
        };

        // Act
        var result = filter.Authorize();

        // Assert
        result.Should().BeFalse();
        httpContext.Response.StatusCode.Should().Be(401);
        httpContext.Response.Headers[HeaderNames.WWWAuthenticate].Should().Contain("Basic realm=\"Serilog UI\"");
    }
}