using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog.Ui.Core.Models;

namespace Serilog.Ui.Common.Tests.DataSamples
{
    public static class SerilogSinkFakeDataProducer
    {
        public static LogModelPropsCollector Logs(ILogger logger)
        {
            var logs = new List<LogModel>();
            // information
            logger.Information("90 MyTestSearchItem");
            logs.Add(Spawn("Information", 90, "MyTestSearchItem"));
            Task.Delay(2000).Wait();
            logger.Information("91 AnotherProp");
            logs.Add(Spawn("Information", 91, "AnotherProp"));
            Task.Delay(1000).Wait();
            logger.Information("92 Information");
            logs.Add(Spawn("Information", 92));
            Task.Delay(3000).Wait();

            // warnings
            for (var i = 0; i < 15; i++)
            {
                logger.Warning($"Hello Warning {i}");
                logs.Add(Spawn("Warning", i));
            }

            logger.Information("Hello Information");
            logs.Add(Spawn("Information", 15));
            // debug
            logger.Debug("Hello Debug");
            logs.Add(Spawn("Debug", 16));
            logger.Debug("Hello Debug");
            logs.Add(Spawn("Debug", 17));
            // error
            var exc = new InvalidOperationException();
            logger.Error(exc, "Hello Error");
            logs.Add(Spawn("Error", 18, exc: exc));
            logger.Error(exc, "Hello Error");
            logs.Add(Spawn("Error", 19, exc: exc));
            // fatal
            var excFatal = new AccessViolationException();
            logger.Fatal(excFatal, "Hello Fatal");
            logs.Add(Spawn("Fatal", 20, exc: excFatal));
            logger.Fatal(excFatal, "Hello Fatal");
            logs.Add(Spawn("Fatal", 21, exc: excFatal));
            // verbose
            logger.Verbose("Hello Verbose");
            logs.Add(Spawn("Verbose", 22));
            logger.Verbose("Hello Verbose");
            logs.Add(Spawn("Verbose", 23));

            return new LogModelPropsCollector(logs);
        }

        private static LogModel Spawn(string level, int rowNum, string? messageOverride = null, Exception? exc = null)
            => new LogModel
            {
                Exception = exc?.ToString(),
                Level = level,
                Message = $"{rowNum} {messageOverride ?? level}",
                Properties = PropertiesFaker.SerializedProperties,
                PropertyType = "json",
                Timestamp = DateTime.UtcNow
            }.SetRowNo(rowNum, -1);
    }
}