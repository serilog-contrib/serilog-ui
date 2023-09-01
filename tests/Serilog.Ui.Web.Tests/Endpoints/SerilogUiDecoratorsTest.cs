using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Serilog.Ui.Web;
using Serilog.Ui.Web.Authorization;
using Serilog.Ui.Web.Endpoints;
using Serilog.Ui.Web.Tests.Authorization;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Ui.Web.Tests.Endpoints
{
    [Trait("Ui-Api-Decorators", "Web")]
    public class SerilogUiDecoratorsTest
    {
        private readonly AuthorizationFilterService authMock;
        private readonly ISerilogUiAppRoutes appRoutesMock;
        private readonly ISerilogUiEndpoints endpointMock;
        private readonly SerilogUiAppRoutesDecorator sutRoutesDecorator;
        private readonly SerilogUiEndpointsDecorator sutEndpointsDecorator;

        public SerilogUiDecoratorsTest()
        {
            authMock = new AuthorizationFilterService();
            appRoutesMock = Substitute.For<ISerilogUiAppRoutes>();
            endpointMock = Substitute.For<ISerilogUiEndpoints>();
            appRoutesMock.GetHome(Arg.Any<HttpContext>());
            appRoutesMock.RedirectHome(Arg.Any<HttpContext>());
            endpointMock.GetLogs(Arg.Any<HttpContext>());
            endpointMock.GetApiKeys(Arg.Any<HttpContext>());

            sutRoutesDecorator = new SerilogUiAppRoutesDecorator(appRoutesMock, authMock);
            sutEndpointsDecorator = new SerilogUiEndpointsDecorator(endpointMock, authMock);
        }

        [Fact]
        public async Task It_forwards_the_call_to_app_endpoints_on_success_authentication()
        {
            sutEndpointsDecorator.SetOptions(new());

            await sutEndpointsDecorator.GetLogs(new DefaultHttpContext());
            await sutEndpointsDecorator.GetApiKeys(new DefaultHttpContext());

            await endpointMock.Received().GetLogs(Arg.Any<HttpContext>());
            await endpointMock.Received().GetApiKeys(Arg.Any<HttpContext>());
        }

        [Fact]
        public async Task It_forwards_the_call_to_app_routes_when_unauth_page_access_is_enabled()
        {
            sutRoutesDecorator.SetOptions(new() { });
            await sutRoutesDecorator.GetHome(new DefaultHttpContext());
            await sutRoutesDecorator.RedirectHome(new DefaultHttpContext());

            await appRoutesMock.Received().GetHome(Arg.Any<HttpContext>());
            await appRoutesMock.Received().RedirectHome(Arg.Any<HttpContext>());
        }

        [Fact]
        public async Task It_blocks_the_call_on_failed_authentication()
        {
            var uiOpts = new UiOptions() { Authorization = new() { RunAuthorizationFilterOnAppRoutes = true } };
            uiOpts.Authorization.Filters = new IUiAuthorizationFilter[] { new ForbidLocalRequestFilter() };
            sutRoutesDecorator.SetOptions(uiOpts);
            sutEndpointsDecorator.SetOptions(uiOpts);

            var defaultHttp = new DefaultHttpContext();
            await sutRoutesDecorator.RedirectHome(defaultHttp);
            defaultHttp.Response.StatusCode.Should().Be(403);
            await appRoutesMock.DidNotReceive().RedirectHome(Arg.Any<HttpContext>());

            var defaultHttp2 = new DefaultHttpContext();
            await sutEndpointsDecorator.GetLogs(defaultHttp2);
            defaultHttp2.Response.StatusCode.Should().Be(403);
            await endpointMock.DidNotReceive().GetLogs(Arg.Any<HttpContext>());

            var defaultHttp3 = new DefaultHttpContext();
            await sutEndpointsDecorator.GetApiKeys(defaultHttp3);
            defaultHttp3.Response.StatusCode.Should().Be(403);
            await endpointMock.DidNotReceive().GetApiKeys(Arg.Any<HttpContext>());
        }

        [Fact]
        public async Task It_blocks_the_GetHome_on_failed_authentication_with_custom_delegate()
        {
            var uiOpts = new UiOptions() { Authorization = new() { RunAuthorizationFilterOnAppRoutes = true } };
            uiOpts.Authorization.Filters = new IUiAuthorizationFilter[] { new ForbidLocalRequestFilter() };
            sutRoutesDecorator.SetOptions(uiOpts);
            sutEndpointsDecorator.SetOptions(uiOpts);

            var defaultHttp = new DefaultHttpContext();
            defaultHttp.Response.Body = new MemoryStream();
            await sutRoutesDecorator.GetHome(defaultHttp);

            defaultHttp.Response.StatusCode.Should().Be(403);
            await appRoutesMock.DidNotReceive().GetHome(Arg.Any<HttpContext>());

            defaultHttp.Response.Body.Seek(0, SeekOrigin.Begin);
            var bodyWrite = await new StreamReader(defaultHttp.Response.Body).ReadToEndAsync();
            bodyWrite.Should().Be("<p>You don't have enough permission to access this page!</p>");
        }
    }
}
