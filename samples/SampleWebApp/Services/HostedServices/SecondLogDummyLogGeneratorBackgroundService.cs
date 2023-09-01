using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Settings.Configuration;

namespace SampleWebApp.Services.HostedServices;

/// <summary>
/// A background service that generates some dummy logs every 5 seconds.
/// </summary>
public class SecondLogDummyLogGeneratorBackgroundService : BackgroundService
{
    private readonly IConfiguration _configuration;

    public SecondLogDummyLogGeneratorBackgroundService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Create the logger
        var logger = new LoggerConfiguration()
            .ReadFrom
            .Configuration(_configuration, new ConfigurationReaderOptions() { SectionName = "Serilog2" })
            .CreateLogger()
            .ForContext<SecondLogDummyLogGeneratorBackgroundService>();

        var rand = new Random();

        // Lets not generate too much data per run.
        const int limit = 10;

        var count = 0;
        while (++count <= limit)
        {
            // Stop when the application is stopping.
            stoppingToken.ThrowIfCancellationRequested();

            // Get a random level
            var level = (LogEventLevel)rand.NextInt64((long)LogEventLevel.Fatal + 1L);

            // Get a random number
            var n = rand.NextInt64(0, 101);

            Exception exception = null;

            if (level >= LogEventLevel.Error)
            {
                exception = GetException();
            }

            logger.Write(level, exception, "Here's a random value: {n}", n);

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }

    }

    [DebuggerHidden]
    private static Exception GetException()
    {
        try
        {
            throw new TestException();
        }
        catch (Exception e)
        {
            return e;
        }
    }

    /// <summary>
    /// Feel free to disable breaking on this exception in your debugger settings.
    /// </summary>
    private class TestException : Exception
    {
        public TestException()
            : base("This is a test exception.")
        {
        }
    }
}