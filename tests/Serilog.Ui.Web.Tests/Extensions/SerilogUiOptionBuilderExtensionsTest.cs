using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Interfaces;
using Serilog.Ui.Web.Authorization.Filters;
using Serilog.Ui.Web.Extensions;
using Xunit;

namespace Serilog.Ui.Web.Tests.Extensions;

[Trait("Ui-SerilogUiOptionBuilder", "Web")]
public class SerilogUiOptionBuilderExtensionsTest
{
    private readonly IServiceCollection _services = new ServiceCollection();

    private readonly SerilogUiOptionsBuilder _builder;

    public SerilogUiOptionBuilderExtensionsTest()
    {
        _services.AddHttpContextAccessor();
        _services.AddScoped<IAuthorizationService>(_ => Substitute.For<IAuthorizationService>());
        
        _builder = new SerilogUiOptionsBuilder(_services);
    }

    [Fact]
    public void It_registers_basic_filter()
    {
        // Act
        _builder.AddScopedBasicAuthFilter();
        var serviceProvider = _services.BuildServiceProvider();

        // Assert
        using var scope = serviceProvider.CreateScope();
        scope.ServiceProvider.GetService<IUiAuthorizationFilter>()
            .Should().NotBeNull()
            .And.BeOfType<BasicAuthenticationFilter>();
    }

    [Fact]
    public void It_registers_local_requests_filter()
    {
        // Act
        _builder.AddScopedAuthorizeLocalRequestsAuthFilter();
        var serviceProvider = _services.BuildServiceProvider();

        // Assert
        using var scope = serviceProvider.CreateScope();
        scope.ServiceProvider.GetService<IUiAuthorizationFilter>()
            .Should().NotBeNull()
            .And.BeOfType<LocalRequestsOnlyAuthorizationFilter>();
    }

    [Fact]
    public void It_registers_policy_filter()
    {
        // Act
        _builder.AddScopedPolicyAuthFilter("policy");
        var serviceProvider = _services.BuildServiceProvider();

        // Assert
        using var scope = serviceProvider.CreateScope();
        scope.ServiceProvider.GetService<IUiAsyncAuthorizationFilter>()
            .Should().NotBeNull()
            .And.BeOfType<PolicyAuthorizationFilter>();
    }
}