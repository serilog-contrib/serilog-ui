using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.Core.Interfaces;
using Serilog.Ui.Core.OptionsBuilder;
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
        _services.AddScoped(_ => Substitute.For<IAuthorizationService>());

        _builder = new SerilogUiOptionsBuilder(_services);
    }

    [Fact]
    public void It_registers_async_filter()
    {
        // Act
        _builder.AddScopedAsyncAuthFilter<SampleFilter>();
        var serviceProvider = _services.BuildServiceProvider();

        // Assert
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetService<IUiAsyncAuthorizationFilter>();
        service.Should().NotBeNull().And.BeOfType<SampleFilter>();
        service.As<SampleFilter>().Test.Should().BeNull();
    }

    [Fact]
    public void It_registers_async_filter_with_implementation_factory()
    {
        // Act
        _builder.AddScopedAsyncAuthFilter<SampleFilter>(_ => new SampleFilter("my-test"));
        var serviceProvider = _services.BuildServiceProvider();

        // Assert
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetService<IUiAsyncAuthorizationFilter>();
        service.Should().NotBeNull().And.BeOfType<SampleFilter>();
        service.As<SampleFilter>().Test.Should().Be("my-test");
    }

    [Fact]
    public void It_registers_sync_filter()
    {
        // Act
        _builder.AddScopedSyncAuthFilter<SampleFilter>();
        var serviceProvider = _services.BuildServiceProvider();

        // Assert
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetService<IUiAuthorizationFilter>();
        service.Should().NotBeNull().And.BeOfType<SampleFilter>();
        service.As<SampleFilter>().Test.Should().BeNull();
    }

    [Fact]
    public void It_registers_sync_filter_with_implementation_factory()
    {
        // Act
        _builder.AddScopedSyncAuthFilter<SampleFilter>(_ => new SampleFilter("my-test"));
        var serviceProvider = _services.BuildServiceProvider();

        // Assert
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetService<IUiAuthorizationFilter>();
        service.Should().NotBeNull().And.BeOfType<SampleFilter>();
        service.As<SampleFilter>().Test.Should().Be("my-test");
    }

    [Fact]
    public void It_registers_basic_filter_with_default_implementation()
    {
        // Act
        _builder.AddScopedBasicAuthFilter();
        var config = Substitute.For<IConfiguration>();
        var serviceProvider = _services.AddScoped<IConfiguration>((_) => config).BuildServiceProvider();

        // Assert
        using var scope = serviceProvider.CreateScope();
        scope.ServiceProvider.GetService<IUiAsyncAuthorizationFilter>()
            .Should().NotBeNull()
            .And.BeOfType<BasicAuthenticationFilter>();
        scope.ServiceProvider.GetService<IBasicAuthenticationService>()
            .Should().NotBeNull()
            .And.BeOfType<BasicAuthServiceByConfiguration>();
    }

    [Fact]
    public void It_registers_basic_filter_with_custom_implementation()
    {
        // Act
        _builder.AddScopedBasicAuthFilter<BasicService>();
        var serviceProvider = _services.BuildServiceProvider();

        // Assert
        using var scope = serviceProvider.CreateScope();
        scope.ServiceProvider.GetService<IUiAsyncAuthorizationFilter>()
            .Should().NotBeNull()
            .And.BeOfType<BasicAuthenticationFilter>();
        scope.ServiceProvider.GetService<IBasicAuthenticationService>()
            .Should().NotBeNull()
            .And.BeOfType<BasicService>();
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

    private class SampleFilter(string? test = null) : IUiAuthorizationFilter, IUiAsyncAuthorizationFilter
    {
        public string? Test => test;

        public bool Authorize() => true;

        public Task<bool> AuthorizeAsync() => Task.FromResult(true);
    }

    private class BasicService : IBasicAuthenticationService
    {
        public Task<bool> CanAccessAsync(AuthenticationHeaderValue basicHeader) => Task.FromResult(true);
    }
}