using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Ui.Web.Tests.Utilities;
using Xunit;

namespace Serilog.Ui.Web.Tests.Authorization;

public class AuthorizationFilterDefaultTest : IClassFixture<WebApplicationFactory<WebSampleProgram>>
{
    private readonly HttpClient _client;

    public AuthorizationFilterDefaultTest(WebApplicationFactory<WebSampleProgram> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Local_Requests_Are_Allowed_By_Default()
    {
        // Act
        var response = await _client.GetAsync("/serilog-ui/index.html");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}

public class AuthorizationFilterWithUserImplementationTest : IClassFixture<WebSampleProgramWithForbiddenLocalRequest>
{
    private readonly HttpClient _client;

    public AuthorizationFilterWithUserImplementationTest(WebSampleProgramWithForbiddenLocalRequest factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Local_Requests_Are_Not_Allowed()
    {
        // Act
        var response = await _client.GetAsync("/serilog-ui/index.html");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}