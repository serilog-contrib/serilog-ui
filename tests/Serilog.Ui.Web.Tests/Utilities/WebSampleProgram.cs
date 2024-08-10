using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Sinks.InMemory;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.Web.Endpoints;
using Serilog.Ui.Web.Extensions;
using Serilog.Ui.Web.Models;
using Serilog.Ui.Web.Tests.Utilities.Authorization;
using Serilog.Ui.Web.Tests.Utilities.InMemoryDataProvider;

namespace Serilog.Ui.Web.Tests.Utilities;

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
                    services.AddHttpContextAccessor();
                    Log.Logger = new LoggerConfiguration().WriteTo.InMemory().CreateLogger();
                })
                .ConfigureTestServices(WithTestServices)
                .Configure(WithCustomConfigure);
        }

        protected virtual void WithTestServices(IServiceCollection services)
        {
            services.AddSerilogUi(options => options.UseInMemory());
        }

        protected virtual void WithCustomConfigure(IApplicationBuilder builder)
        {
            builder.UseSerilogUi();
        }
    }

    public class WithMocks : Default
    {
        protected override void WithTestServices(IServiceCollection services)
        {
            base.WithTestServices(services);
            // mock some services
            services.AddScoped<ISerilogUiAppRoutes, FakeAppRoutes>();
            services.AddScoped<ISerilogUiEndpoints, FakeAppRoutes>();
        }

        private class FakeAppRoutes(IHttpContextAccessor contextAccessor) : ISerilogUiAppRoutes, ISerilogUiEndpoints
        {
            public UiOptions? Options { get; set; }

            public bool BlockHomeAccess { get; set; }

            public Task GetApiKeysAsync() => Oper(417);

            public Task GetHomeAsync() => Oper(418);
            public Task GetLogsAsync() => Oper(409);
            public Task RedirectHomeAsync() => Oper(400);
            public void SetOptions(UiOptions options) => Options = options;

            private Task Oper(int statusCode)
            {
                contextAccessor.HttpContext!.Response.StatusCode = statusCode;
                return Task.CompletedTask;
            }
        }

        public class AndCustomOptions : WithMocks
        {
            protected override void WithCustomConfigure(IApplicationBuilder builder)
            {
                builder.UseSerilogUi(ui => ui.WithRoutePrefix("test"));
            }
        }
    }

    public class WithForbidden
    {
        public class Sync : Default
        {
            protected override void WithTestServices(IServiceCollection services)
            {
                services.AddSerilogUi(options => options
                    .AddScopedAsyncAuthFilter<AdmitRequestAsyncFilter>()
                    .AddScopedSyncAuthFilter<ForbidLocalRequestFilter>()
                    .AddScopedSyncAuthFilter<AdmitRequestFilter>()
                    .UseInMemory()
                );
            }

            protected override void WithCustomConfigure(IApplicationBuilder builder)
            {
                builder.UseSerilogUi(options =>
                {
                    options.Authorization.AuthenticationType = AuthenticationType.Jwt;
                    options.Authorization.RunAuthorizationFilterOnAppRoutes = true;
                });
            }
        }

        public class Async : Default
        {
            protected override void WithTestServices(IServiceCollection services)
            {
                services.AddSerilogUi(options => options
                    .AddScopedAsyncAuthFilter<AdmitRequestAsyncFilter>()
                    .AddScopedAsyncAuthFilter<ForbidLocalRequestAsyncFilter>()
                    .AddScopedSyncAuthFilter<AdmitRequestFilter>()
                    .UseInMemory()
                );
            }

            protected override void WithCustomConfigure(IApplicationBuilder builder)
            {
                builder.UseSerilogUi(options =>
                {
                    options.Authorization.AuthenticationType = AuthenticationType.Jwt;
                    options.Authorization.RunAuthorizationFilterOnAppRoutes = true;
                });
            }
        }
    }
}

public class WebSampleProgram
{
    protected WebSampleProgram()
    {
    }

    internal static void Main()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();
        app.Run();
    }
}