using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Serilog.Ui.Web.Tests.Utilities;
using Xunit;

namespace Serilog.Ui.Web.Tests.Authorization;

[Trait("Ui-Authorization", "Web")]
public class AuthorizationDefaultTest(WebAppFactory.Default factory) : IClassFixture<WebAppFactory.Default>
{
    private readonly HttpClient _client = factory.CreateClient();

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