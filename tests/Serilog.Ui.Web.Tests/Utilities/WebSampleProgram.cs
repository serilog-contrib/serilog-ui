using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.InMemory;
using Serilog.Ui.Web;
using Serilog.Ui.Web.Tests.Authorization;
using Serilog.Ui.Web.Tests.SerilogInMemoryDataProvider;
using System.IO;

namespace Ui.Web.Tests.Utilities;

public class WebSampleProgram
{
    internal static void Main()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            WebRootPath = Directory.GetCurrentDirectory()
        });

        builder.Services.AddEndpointsApiExplorer();

        Log.Logger = new LoggerConfiguration().WriteTo.InMemory().CreateLogger();
        builder.Host.UseSerilog();
        builder.Services.AddSerilogUi(options => options.UseInMemory());

        var app = builder.Build();
        app.UseSerilogUi();

        app.Run();
    }
}

public class WebSampleProgramDefaultFactory : WebApplicationFactory<WebSampleProgram> { }

public class WebSampleProgramWithForbiddenLocalRequest : WebApplicationFactory<WebSampleProgram>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.Configure(appBuilder =>
        {
            appBuilder.UseSerilogUi(options =>
            {
                options.Authorization.AuthenticationType = AuthenticationType.Jwt;
                options.Authorization.Filters = new[]
                {
                        new ForbidLocalRequestFilter()
                    };
            });
        });
    }
}