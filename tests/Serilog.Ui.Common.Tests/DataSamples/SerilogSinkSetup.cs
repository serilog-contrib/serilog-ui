using System;
using Serilog.Events;

namespace Serilog.Ui.Common.Tests.DataSamples;

public class SerilogSinkSetup
{
    private readonly LoggerConfiguration _loggerConfig;

    public SerilogSinkSetup(Action<LoggerConfiguration> setupSinkAction)
    {
        _loggerConfig = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.WithEnvironmentName()
            .Enrich.WithEnvironmentUserName()
            .Enrich.AtLevel(LogEventLevel.Warning, p =>
            {
                p.WithProperty("SampleBool", true);
                p.WithProperty("SampleDate", new DateTime(2022, 01, 15, 10, 00, 00));
            });
        setupSinkAction(_loggerConfig);
    }

    public LogModelPropsCollector InitializeLogs()
    {
        using var logger = _loggerConfig.CreateLogger();
        return SerilogSinkFakeDataProducer.Logs(logger);
    }
}