using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Serilog.Ui.Common.Tests.DataSamples
{
    public static class ElasticSearchLogModelFaker
    {
        public static LogModelPropsCollector Logs(ILogger logger)
        {
            var logs = new List<LogModel>();
            logger.Information("90 MyTestSearchItem");
            logs.Add(Spawn("Information", 90, "MyTestSearchItem"));
            Task.Delay(2000).Wait();
            logger.Information("91 AnotherProp");
            logs.Add(Spawn("Information", 91, "AnotherProp"));
            Task.Delay(1000).Wait();
            logger.Information("92 Information");
            logs.Add(Spawn("Information", 92));
            Task.Delay(3000).Wait();

            for (int i = 0; i < 15; i++)
            {
                logger.Warning($"Hello Warning {i}");
                logs.Add(Spawn("Warning", i));
            }

            logger.Information("Hello Information");
            logs.Add(Spawn("Information", 15));
            logger.Debug("Hello Debug");
            logs.Add(Spawn("Debug", 16));
            logger.Error("Hello Error");
            logs.Add(Spawn("Error", 17));
            logger.Fatal("Hello Fatal");
            logs.Add(Spawn("Information", 18));

            return new LogModelPropsCollector(logs);
        }

        private static LogModel Spawn(string level, int rowNum, string messageOverride = null)
            => new()
            {
                Exception = null,
                Level = level,
                Message = $"{rowNum} {messageOverride ?? level}",
                Properties = PropertiesFaker.SerializedProperties,
                PropertyType = "json",
                RowNo = rowNum,
                Timestamp = DateTime.UtcNow
            };
    }
}
