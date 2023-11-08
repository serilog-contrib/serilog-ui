using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace Serilog.Ui.Web.Authorization.Tests;

public class BasicAuthenticationFilterTests
{
    [Fact]
    public async Task Authorize_WithValidCredentials_ShouldReturnTrue()
    {
        // Arrange
        var filter = new BasicAuthenticationFilter
        {
            User = "User",
            Pass = "P@ss"
        };

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "Basic VXNlcjpQQHNz"; // Base64 encoded "User:P@ss"

        // Act
        var result = filter.Authorize(httpContext);
        var authCookie = httpContext.Response.GetTypedHeaders().SetCookie.FirstOrDefault(sc => sc.Name == BasicAuthenticationFilter.AuthenticationCookieName);

        // Assert
        result.Should().BeTrue();
        authCookie.Should().NotBeNull();
    }

    [Fact]
    public async Task Authorize_WithInvalidCredentials_ShouldReturnFalse()
    {
        // Arrange
        var filter = new BasicAuthenticationFilter
        {
            User = "User",
            Pass = "P@ss"
        };

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "Basic QWRtaW46dXNlcg=="; // Base64 encoded "Admin:user"

        // Act
        var result = filter.Authorize(httpContext);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Authorize_WithMissingAuthorizationHeader_ShouldSetChallengeResponse()
    {
        // Arrange
        var filter = new BasicAuthenticationFilter
        {
            User = "User",
            Pass = "P@ss"
        };

        var httpContext = new DefaultHttpContext();

        // Act
        var result = filter.Authorize(httpContext);

        // Assert
        result.Should().BeFalse();
        httpContext.Response.StatusCode.Should().Be(401);
        httpContext.Response.Headers[HeaderNames.WWWAuthenticate].Should().Contain("Basic realm=\"Serilog UI\"");
    }
}