using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ClearExtensions;
using Serilog.Ui.Core.Interfaces;
using Serilog.Ui.Web.Authorization;
using Serilog.Ui.Web.Endpoints;
using Serilog.Ui.Web.Models;
using Serilog.Ui.Web.Tests.Utilities.Authorization;
using Xunit;

namespace Serilog.Ui.Web.Tests.Endpoints
{
    [Trait("Ui-Api-Decorators", "Web")]
    public class SerilogUiDecoratorsTest
    {
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly List<IUiAuthorizationFilter> _syncFilters = [];

        private readonly ISerilogUiAppRoutes _appRoutesMock;

        private readonly ISerilogUiEndpoints _endpointMock;

        private readonly SerilogUiAppRoutesDecorator _sutRoutesDecorator;

        private readonly SerilogUiEndpointsDecorator _sutEndpointsDecorator;

        private DefaultHttpContext _context;

        public SerilogUiDecoratorsTest()
        {
            _context = new DefaultHttpContext();
            _contextAccessor = Substitute.For<IHttpContextAccessor>();
            _contextAccessor.HttpContext.Returns(_context);

            var authMock = new AuthorizationFilterService(_contextAccessor, _syncFilters, []);

            _appRoutesMock = Substitute.For<ISerilogUiAppRoutes>();
            _endpointMock = Substitute.For<ISerilogUiEndpoints>();

            _sutRoutesDecorator = new SerilogUiAppRoutesDecorator(_appRoutesMock, authMock);
            _sutEndpointsDecorator = new SerilogUiEndpointsDecorator(_endpointMock, authMock);
        }

        [Fact]
        public async Task It_forwards_the_call_to_GetLogs_on_success_authentication()
        {
            // Arrange
            _sutEndpointsDecorator.SetOptions(new UiOptions());

            // Act
            await _sutEndpointsDecorator.GetLogsAsync();

            // Assert
            await _endpointMock.Received().GetLogsAsync();
        }

        [Fact]
        public async Task It_forwards_the_call_to_GetKeys_on_success_authentication()
        {
            // Arrange
            _sutEndpointsDecorator.SetOptions(new UiOptions());

            // Act
            await _sutEndpointsDecorator.GetApiKeysAsync();

            // Assert
            await _endpointMock.Received().GetApiKeysAsync();
        }

        [Fact]
        public async Task It_forwards_the_call_to_GetHome_when_unauthorized_page_access_is_enabled()
        {
            // Arrange
            _sutRoutesDecorator.SetOptions(new UiOptions());

            // Act
            await _sutRoutesDecorator.GetHomeAsync();

            // Assert
            await _appRoutesMock.Received().GetHomeAsync();
        }

        [Fact]
        public async Task It_forwards_the_call_to_RedirectHome_when_unauthorized_page_access_is_enabled()
        {
            // Arrange
            _sutRoutesDecorator.SetOptions(new UiOptions());

            // Act
            await _sutRoutesDecorator.RedirectHomeAsync();

            // Assert
            await _appRoutesMock.Received().RedirectHomeAsync();
        }

        [Fact]
        public async Task It_blocks_the_call_on_failed_authentication()
        {
            // Arrange
            var uiOpts = new UiOptions { Authorization = new() { RunAuthorizationFilterOnAppRoutes = true } };
            _syncFilters.Add(new ForbidLocalRequestFilter(_contextAccessor));
            _sutRoutesDecorator.SetOptions(uiOpts);
            _sutEndpointsDecorator.SetOptions(uiOpts);

            // Act
            await _sutRoutesDecorator.RedirectHomeAsync();
            // Assert
            _context.Response.StatusCode.Should().Be(403);
            await _appRoutesMock.DidNotReceive().RedirectHomeAsync();

            // Arrange
            _contextAccessor.ClearSubstitute();
            var cleanContext = new DefaultHttpContext();
            _contextAccessor.HttpContext.Returns(cleanContext);
            
            // Act
            await _sutEndpointsDecorator.GetLogsAsync();
            // Assert
            _context.Response.StatusCode.Should().Be(403);
            await _endpointMock.DidNotReceive().GetLogsAsync();
            
            // Arrange
            _contextAccessor.ClearSubstitute();
            var cleanContext2 = new DefaultHttpContext();
            _contextAccessor.HttpContext.Returns(cleanContext2);
            
            // Act
            await _sutEndpointsDecorator.GetApiKeysAsync();
            // Assert
            _context.Response.StatusCode.Should().Be(403);
            await _endpointMock.DidNotReceive().GetApiKeysAsync();
        }

        [Fact]
        public async Task It_blocks_the_GetHome_on_failed_authentication_with_custom_delegate()
        {
            // Arrange
            var uiOpts = new UiOptions { Authorization = new() { RunAuthorizationFilterOnAppRoutes = true } };
            _syncFilters.Add(new ForbidLocalRequestFilter(_contextAccessor));
            _sutRoutesDecorator.SetOptions(uiOpts);
            _sutEndpointsDecorator.SetOptions(uiOpts);

            // Act
            var context = new DefaultHttpContext
            {
                Response =
                {
                    Body = new MemoryStream()
                }
            };
            _contextAccessor.HttpContext.Returns(context);
            await _sutRoutesDecorator.GetHomeAsync();

            // Assert
            context.Response.StatusCode.Should().Be(403);
            await _appRoutesMock.DidNotReceive().GetHomeAsync();

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var bodyWrite = await new StreamReader(context.Response.Body).ReadToEndAsync();
            bodyWrite.Should().Be("<p>You don't have enough permission to access this page!</p>");
        }
    }
}