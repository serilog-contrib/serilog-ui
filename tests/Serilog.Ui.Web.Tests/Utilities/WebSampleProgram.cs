using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.InMemory;
using Serilog.Ui.Web;
using Serilog.Ui.Web.Authorization;
using Serilog.Ui.Web.Endpoints;
using Serilog.Ui.Web.Tests.Authorization;
using System.IO;
using System.Threading.Tasks;
using Ui.Web.Tests.Utilities.InMemoryDataProvider;

namespace Ui.Web.Tests.Utilities;

public class WebAppFactory
{
    public class Default : WebApplicationFactory<WebSampleProgram>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseWebRoot(Directory.GetCurrentDirectory())
                .UseTestServer()
                .ConfigureServices(services =>
                {
                    services.AddEndpointsApiExplorer();
                    Log.Logger = new LoggerConfiguration().WriteTo.InMemory().CreateLogger();
                    services.AddSerilogUi(options => options.UseInMemory());
                })
                .ConfigureTestServices(WithTestServices)
                .Configure(WithCustomConfigure);
        }

        protected virtual void WithTestServices(IServiceCollection services) { }
        protected virtual void WithCustomConfigure(IApplicationBuilder builder)
        {
            builder.UseSerilogUi();
        }
    }
    public class WithMocks : Default
    {
        protected override void WithTestServices(IServiceCollection services)
        {
            // mock some services
            services.AddScoped<ISerilogUiAppRoutes, FakeAppRoutes>();
            services.AddScoped<ISerilogUiEndpoints, FakeAppRoutes>();
        }

        private class FakeAppRoutes : ISerilogUiAppRoutes, ISerilogUiEndpoints
        {
            public UiOptions? Options { get; set; }
            public Task GetApiKeys(HttpContext httpContext) => Oper(httpContext, 417);
            public Task GetHome(HttpContext httpContext) => Oper(httpContext, 418);
            public Task GetLogs(HttpContext httpContext) => Oper(httpContext, 409);
            public Task RedirectHome(HttpContext httpContext) => Oper(httpContext, 400);
            public void SetOptions(UiOptions options) => Options = options;
            private static Task Oper(HttpContext httpContext, int statusCode)
            {
                httpContext.Response.StatusCode = statusCode;
                return Task.CompletedTask;
            }
        }

        public class AndCustomOptions : WithMocks
        {
            protected override void WithCustomConfigure(IApplicationBuilder builder)
            {
                builder.UseSerilogUi(ui => { ui.RoutePrefix = "test"; });
            }
        }
    }

    public class WithForbidden
    {
        public class Sync : Default
        {
            protected override void WithCustomConfigure(IApplicationBuilder builder)
            {
                builder.UseSerilogUi(options =>
                {
                    options.Authorization.AuthenticationType = AuthenticationType.Jwt;
                    options.Authorization.Filters = new IUiAuthorizationFilter[]
                    {
                        new ForbidLocalRequestFilter(),
                        new AdmitRequestFilter()
                    };
                    options.Authorization.AsyncFilters = new[]
                    {
                        new AdmitRequestAsyncFilter()
                    };
                });
            }
        }

        public class Async : Default
        {
            protected override void WithCustomConfigure(IApplicationBuilder builder)
            {
                builder.UseSerilogUi(options =>
                {
                    options.Authorization.AuthenticationType = AuthenticationType.Jwt;
                    builder.UseSerilogUi(options =>
                    {
                        options.Authorization.AuthenticationType = AuthenticationType.Jwt;
                        options.Authorization.Filters = new[]
                        {
                        new AdmitRequestFilter()
                    };
                        options.Authorization.AsyncFilters = new IUiAsyncAuthorizationFilter[]
                        {
                        new ForbidLocalRequestAsyncFilter(),
                        new AdmitRequestAsyncFilter()
                    };
                    });
                });
            }
        }
    }
}

public class WebSampleProgram
{
    protected WebSampleProgram() { }

    internal static void Main()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();
        app.Run();
    }
}
