using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Serilog.Ui.Web.Tests.Authorization;

public class AuthorizationFilterTests : IClassFixture<WebApplicationFactory<Program1>>
{
    private readonly HttpClient _client;

    public AuthorizationFilterTests(WebApplicationFactory<Program1> factory)
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

public class AuthorizationFilterTests2 : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthorizationFilterTests2(CustomWebApplicationFactory factory)
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

public class CustomWebApplicationFactory : WebApplicationFactory<Program1>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.Configure(appBuilder =>
        {
            appBuilder.UseSerilogUi(options =>
            {
                options.Authorization.AuthenticationType = AuthenticationType.Jwt;
                options.Authorization.Filters = new[]
                {
                    new ForbidLocalRequestFilter()
                };
            });
        });
    }
}