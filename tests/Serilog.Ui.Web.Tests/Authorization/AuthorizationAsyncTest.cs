using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Serilog.Ui.Web.Tests.Utilities;
using Xunit;

namespace Serilog.Ui.Web.Tests.Authorization;

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