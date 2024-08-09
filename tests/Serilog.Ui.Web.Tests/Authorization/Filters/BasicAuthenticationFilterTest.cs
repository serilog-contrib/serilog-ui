using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Serilog.Ui.Web.Authorization.Filters;
using Xunit;

namespace Serilog.Ui.Web.Tests.Authorization.Filters;

[Trait("Ui-Authorization-Filters", "Web")]
public class BasicAuthenticationFilterTest
{
    [Fact]
    public async Task Authorize_returns_true_on_not_basic_authorization()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Authorization = "Bearer my-token";
        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        contextAccessor.HttpContext.Returns(httpContext);

        var filter = new BasicAuthenticationFilter(contextAccessor, null!);

        // Act
        var result = await filter.AuthorizeAsync();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Authorize_with_implementation_returns_implementation_result(bool isAuth)
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Authorization = "Basic something";
        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        contextAccessor.HttpContext.Returns(httpContext);
        var basicService = Substitute.For<IBasicAuthenticationService>();
        basicService.CanAccessAsync(Arg.Any<AuthenticationHeaderValue>()).Returns(Task.FromResult(isAuth));

        var filter = new BasicAuthenticationFilter(contextAccessor, basicService);

        // Act
        var result = await filter.AuthorizeAsync();

        // Assert
        result.Should().Be(isAuth);
        await basicService.Received().CanAccessAsync(Arg.Any<AuthenticationHeaderValue>());
    }
}