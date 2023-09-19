using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Serilog.Ui.Web;
using Serilog.Ui.Web.Authorization;
using Serilog.Ui.Web.Endpoints;
using System.IO;
using System.Threading.Tasks;
using Ui.Web.Tests.Utilities.Authorization;
using Xunit;

namespace Ui.Web.Tests.Endpoints
{
    [Trait("Ui-Api-Decorators", "Web")]
    public class SerilogUiDecoratorsTest
    {
        private readonly AuthorizationFilterService _authMock;
        private readonly ISerilogUiAppRoutes _appRoutesMock;
        private readonly ISerilogUiEndpoints _endpointMock;
        private readonly SerilogUiAppRoutesDecorator _sutRoutesDecorator;
        private readonly SerilogUiEndpointsDecorator _sutEndpointsDecorator;

        public SerilogUiDecoratorsTest()
        {
            _authMock = new AuthorizationFilterService();
            _appRoutesMock = Substitute.For<ISerilogUiAppRoutes>();
            _endpointMock = Substitute.For<ISerilogUiEndpoints>();
            _appRoutesMock.GetHome(Arg.Any<HttpContext>());
            _appRoutesMock.RedirectHome(Arg.Any<HttpContext>());
            _endpointMock.GetLogs(Arg.Any<HttpContext>());
            _endpointMock.GetApiKeys(Arg.Any<HttpContext>());

            _sutRoutesDecorator = new SerilogUiAppRoutesDecorator(_appRoutesMock, _authMock);
            _sutEndpointsDecorator = new SerilogUiEndpointsDecorator(_endpointMock, _authMock);
        }

        [Fact]
        public async Task It_forwards_the_call_to_app_endpoints_on_success_authentication()
        {
            // Arrange
            _sutEndpointsDecorator.SetOptions(new());

            // Act
            await _sutEndpointsDecorator.GetLogs(new DefaultHttpContext());
            await _sutEndpointsDecorator.GetApiKeys(new DefaultHttpContext());

            // Assert
            await _endpointMock.Received().GetLogs(Arg.Any<HttpContext>());
            await _endpointMock.Received().GetApiKeys(Arg.Any<HttpContext>());
        }

        [Fact]
        public async Task It_forwards_the_call_to_app_routes_when_unauth_page_access_is_enabled()
        {
            // Arrange
            _sutRoutesDecorator.SetOptions(new() { });

            // Act
            await _sutRoutesDecorator.GetHome(new DefaultHttpContext());
            await _sutRoutesDecorator.RedirectHome(new DefaultHttpContext());

            // Assert
            await _appRoutesMock.Received().GetHome(Arg.Any<HttpContext>());
            await _appRoutesMock.Received().RedirectHome(Arg.Any<HttpContext>());
        }

        [Fact]
        public async Task It_blocks_the_call_on_failed_authentication()
        {
            // Arrange
            var uiOpts = new UiOptions() { Authorization = new() { RunAuthorizationFilterOnAppRoutes = true } };
            uiOpts.Authorization.Filters = new IUiAuthorizationFilter[] { new ForbidLocalRequestFilter() };
            _sutRoutesDecorator.SetOptions(uiOpts);
            _sutEndpointsDecorator.SetOptions(uiOpts);

            // Act
            var defaultHttp = new DefaultHttpContext();
            await _sutRoutesDecorator.RedirectHome(defaultHttp);
            // Assert
            defaultHttp.Response.StatusCode.Should().Be(403);
            await _appRoutesMock.DidNotReceive().RedirectHome(Arg.Any<HttpContext>());

            // Act
            var defaultHttp2 = new DefaultHttpContext();
            await _sutEndpointsDecorator.GetLogs(defaultHttp2);
            // Assert
            defaultHttp2.Response.StatusCode.Should().Be(403);
            await _endpointMock.DidNotReceive().GetLogs(Arg.Any<HttpContext>());

            // Act
            var defaultHttp3 = new DefaultHttpContext();
            await _sutEndpointsDecorator.GetApiKeys(defaultHttp3);
            // Assert
            defaultHttp3.Response.StatusCode.Should().Be(403);
            await _endpointMock.DidNotReceive().GetApiKeys(Arg.Any<HttpContext>());
        }

        [Fact]
        public async Task It_blocks_the_GetHome_on_failed_authentication_with_custom_delegate()
        {
            // Arrange
            var uiOpts = new UiOptions() { Authorization = new() { RunAuthorizationFilterOnAppRoutes = true } };
            uiOpts.Authorization.Filters = new IUiAuthorizationFilter[] { new ForbidLocalRequestFilter() };
            _sutRoutesDecorator.SetOptions(uiOpts);
            _sutEndpointsDecorator.SetOptions(uiOpts);

            // Act
            var defaultHttp = new DefaultHttpContext();
            defaultHttp.Response.Body = new MemoryStream();
            await _sutRoutesDecorator.GetHome(defaultHttp);

            // Assert
            defaultHttp.Response.StatusCode.Should().Be(403);
            await _appRoutesMock.DidNotReceive().GetHome(Arg.Any<HttpContext>());

            defaultHttp.Response.Body.Seek(0, SeekOrigin.Begin);
            var bodyWrite = await new StreamReader(defaultHttp.Response.Body).ReadToEndAsync();
            bodyWrite.Should().Be("<p>You don't have enough permission to access this page!</p>");
        }
    }
}
