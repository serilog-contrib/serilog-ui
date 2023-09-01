using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Ui.Web.Tests.Utilities;
using Xunit;

namespace Serilog.Ui.Web.Tests.Authorization;


[Trait("Ui-Authorization", "Web")]
public class AuthorizationDefaultTest : IClassFixture<WebAppFactory.Default>
{
    private readonly HttpClient _client;

    public AuthorizationDefaultTest(WebAppFactory.Default factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Local_Requests_Are_Allowed_By_Default()
    {
        // Act
        var response = await _client.GetAsync("/serilog-ui/index.html");
        var result = response.EnsureSuccessStatusCode;

        // Assert
        result.Should().NotThrow<HttpRequestException>();
    }
}

[Trait("Ui-Authorization", "Web")]
public class AuthorizationSyncTest : IClassFixture<WebAppFactory.WithForbidden.Sync>
{
    private readonly HttpClient _client;

    public AuthorizationSyncTest(WebAppFactory.WithForbidden.Sync factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Local_Requests_Are_Not_Allowed_By_Sync_Filters()
    {
        // Act
        var response = await _client.GetAsync("/serilog-ui/index.html");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}

[Trait("Ui-Authorization", "Web")]
public class AuthorizationAsyncTest : IClassFixture<WebAppFactory.WithForbidden.Async>
{
    private readonly HttpClient _client;

    public AuthorizationAsyncTest(WebAppFactory.WithForbidden.Async factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Local_Requests_Are_Not_Allowed_By_Async_Filters()
    {
        // Act
        var response = await _client.GetAsync("/serilog-ui/index.html");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}