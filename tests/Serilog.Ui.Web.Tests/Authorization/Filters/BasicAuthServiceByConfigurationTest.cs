using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Serilog.Ui.Web.Authorization.Filters;
using Xunit;

namespace Serilog.Ui.Web.Tests.Authorization.Filters;

[Trait("Ui-Authorization-Filters", "Web")]
public class BasicAuthServiceByConfigurationTest
{
    [Fact]
    public async Task CanAccessAsync_returns_true_with_valid_credentials()
    {
        // Arrange
        const string header = "Basic QWRtaW46dXNlcg=="; // Base64 encoded "Admin:user"
        var headerValue = AuthenticationHeaderValue.Parse(header);
        var config = new ConfigurationManager()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["SerilogUi:UserName"] = "Admin", ["SerilogUi:Password"] = "user" })
            .Build();
        var basicService = new BasicAuthServiceByConfiguration(config);

        // Act
        var result = await basicService.CanAccessAsync(headerValue);

        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task CanAccessAsync_returns_false_with_invalid_credentials()
    {
        // Arrange
        const string header = "Basic QWRtaW46dXNlcg=="; // Base64 encoded "Admin:user"
        var headerValue = AuthenticationHeaderValue.Parse(header);
        var config = new ConfigurationManager()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["SerilogUi:UserName"] = "Admin", ["SerilogUi:Password"] = "Pwd" })
            .Build();
        var basicService = new BasicAuthServiceByConfiguration(config);

        // Act
        var result = await basicService.CanAccessAsync(headerValue);

        // Assert
        result.Should().BeFalse();
    }
}