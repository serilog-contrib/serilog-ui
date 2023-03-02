using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Sinks.InMemory;
using Serilog.Ui.Web.Tests.SerilogInMemoryDataProvider;
using System.IO;

namespace Serilog.Ui.Web.Tests.Authorization;

public class Program1
{
    internal static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            WebRootPath = Directory.GetCurrentDirectory()
        });

        builder.Services.AddEndpointsApiExplorer();

        Log.Logger = new LoggerConfiguration().WriteTo.InMemory().CreateLogger();
        builder.Host.UseSerilog();
        builder.Services.AddSerilogUi(options => options.UseImMemory());

        var app = builder.Build();
        app.UseSerilogUi();

        app.Run();
    }
}