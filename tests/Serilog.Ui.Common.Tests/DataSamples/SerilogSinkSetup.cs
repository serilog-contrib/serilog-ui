using System;

namespace Serilog.Ui.Common.Tests.DataSamples;

public class SerilogSinkSetup
{
    private readonly LoggerConfiguration _loggerConfig;

    public SerilogSinkSetup(Action<LoggerConfiguration> setupSinkAction)
    {
        _loggerConfig = new LoggerConfiguration()
            .MinimumLevel.Verbose();
        setupSinkAction(_loggerConfig);
    }

    public LogModelPropsCollector InitializeLogs()
    {
        using var logger = _loggerConfig.CreateLogger();
        return SerilogSinkFakeDataProducer.Logs(logger);
    }
}