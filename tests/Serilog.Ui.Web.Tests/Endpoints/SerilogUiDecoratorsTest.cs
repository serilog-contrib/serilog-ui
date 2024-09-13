using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Serilog.Ui.Core.Interfaces;
using Serilog.Ui.Core.Models.Options;
using Serilog.Ui.Web.Authorization;
using Serilog.Ui.Web.Endpoints;
using Serilog.Ui.Web.Models;
using Serilog.Ui.Web.Tests.Utilities.Authorization;
using Xunit;

namespace Serilog.Ui.Web.Tests.Endpoints;

[Trait("Ui-Api-Decorators", "Web")]
public class SerilogUiDecoratorsTest
{
    private readonly IHttpContextAccessor _contextAccessor = Substitute.For<IHttpContextAccessor>();

    private readonly List<IUiAuthorizationFilter> _syncFilters = [];

    private readonly ISerilogUiAppRoutes _appRoutesMock = Substitute.For<ISerilogUiAppRoutes>();

    private readonly ISerilogUiEndpoints _endpointMock = Substitute.For<ISerilogUiEndpoints>();

    private readonly SerilogUiAppRoutesDecorator _sutRoutesDecorator;

    private readonly SerilogUiEndpointsDecorator _sutEndpointsDecorator;

    public SerilogUiDecoratorsTest()
    {
        var (serilogUiAppRoutesDecorator, serilogUiEndpointsDecorator) = CreateTarget();
        _sutRoutesDecorator = serilogUiAppRoutesDecorator;
        _sutEndpointsDecorator = serilogUiEndpointsDecorator;
    }

    [Fact]
    public async Task It_forwards_the_call_to_GetLogs_on_success_authentication()
    {
        // Arrange
        _sutEndpointsDecorator.SetOptions(GetOptions());

        // Act
        await _sutEndpointsDecorator.GetLogsAsync();

        // Assert
        await _endpointMock.Received().GetLogsAsync();
    }

    [Fact]
    public async Task It_forwards_the_call_to_GetKeys_on_success_authentication()
    {
        // Arrange
        _sutEndpointsDecorator.SetOptions(GetOptions());

        // Act
        await _sutEndpointsDecorator.GetApiKeysAsync();

        // Assert
        await _endpointMock.Received().GetApiKeysAsync();
    }

    [Fact]
    public async Task It_forwards_the_call_to_GetHome_when_unauthorized_page_access_is_enabled()
    {
        // Arrange
        _sutRoutesDecorator.SetOptions(GetOptions());

        // Act
        await _sutRoutesDecorator.GetHomeAsync();

        // Assert
        await _appRoutesMock.Received().GetHomeAsync();
    }

    [Fact]
    public async Task It_forwards_the_call_to_RedirectHome_always()
    {
        // Arrange
        var options = GetOptions();
        _sutRoutesDecorator.SetOptions(options);

        // Act
        await _sutRoutesDecorator.RedirectHomeAsync();

        options.EnableAuthorizationOnAppRoutes();
        await _sutRoutesDecorator.RedirectHomeAsync();

        // Assert
        await _appRoutesMock.Received(2).RedirectHomeAsync();
    }

    [Fact]
    public async Task It_blocks_the_call_on_failed_authentication()
    {
        // Arrange: reset target to register the new HttpContext variable
        var uiOpts = new UiOptions(new ProvidersOptions()).EnableAuthorizationOnAppRoutes();

        _syncFilters.Add(new ForbidLocalRequestFilter(_contextAccessor));
        var (_, logsEndpointDecorator) = CreateTarget();
        logsEndpointDecorator.SetOptions(uiOpts);

        // Act
        await logsEndpointDecorator.GetLogsAsync();

        // Assert
        _contextAccessor.HttpContext!.Response.StatusCode.Should().Be(403);
        await _endpointMock.DidNotReceive().GetLogsAsync();

        // Arrange: reset target to register the new HttpContext variable
        var (_, apiKeysEndpointDecorator) = CreateTarget();
        apiKeysEndpointDecorator.SetOptions(uiOpts);

        // Act
        await apiKeysEndpointDecorator.GetApiKeysAsync();
        // Assert
        _contextAccessor.HttpContext.Response.StatusCode.Should().Be(403);
        await _endpointMock.DidNotReceive().GetApiKeysAsync();
    }

    [Fact]
    public async Task It_return_GetHome_on_failed_authentication_with_custom_ui_option()
    {
        // Arrange
        var uiOpts = new UiOptions(new ProvidersOptions()).EnableAuthorizationOnAppRoutes();

        _syncFilters.Add(new ForbidLocalRequestFilter(_contextAccessor));
        _sutRoutesDecorator.SetOptions(uiOpts);
        _sutEndpointsDecorator.SetOptions(uiOpts);

        // Act
        var context = new DefaultHttpContext();
        _contextAccessor.HttpContext.Returns(context);

        _appRoutesMock.BlockHomeAccess.Should().BeFalse();

        await _sutRoutesDecorator.GetHomeAsync();

        // Assert
        context.Response.StatusCode.Should().Be(200);
        _appRoutesMock.BlockHomeAccess.Should().BeTrue();

        await _appRoutesMock.Received().GetHomeAsync();
    }

    private static UiOptions GetOptions() => new(new ProvidersOptions());

    private (SerilogUiAppRoutesDecorator, SerilogUiEndpointsDecorator) CreateTarget()
    {
        var context = new DefaultHttpContext();
        _contextAccessor.HttpContext.Returns(context);

        var authMock = new AuthorizationFilterService(_contextAccessor, _syncFilters, []);

        return (
            new SerilogUiAppRoutesDecorator(_contextAccessor, _appRoutesMock, authMock),
            new SerilogUiEndpointsDecorator(_endpointMock, authMock));
    }
}
