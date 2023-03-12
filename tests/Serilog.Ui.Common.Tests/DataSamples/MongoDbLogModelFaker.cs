using MongoDB.Bson;
using Newtonsoft.Json;
using Serilog.Ui.Common.Tests.FakeObjectModels;
using Serilog.Ui.MongoDbProvider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Serilog.Ui.Common.Tests.DataSamples
{
    public static class MongoDbLogModelFaker
    {
        public static (IEnumerable<MongoDbLogModel> logs, LogModelPropsCollector collector) Logs(int generationCount)
        {
            var originalLogs = LogModelFaker.Logs(generationCount);
            var modelCollector = new LogModelPropsCollector(originalLogs);
            return (originalLogs.Select(p => new MongoDbLogModel
            {
                Id = p.RowNo,
                Level = p.Level,
                RenderedMessage = p.Message,
                Timestamp = p.Timestamp,
                UtcTimeStamp = p.Timestamp.ToUniversalTime(),
                Properties = JsonConvert.DeserializeObject<Properties>(p.Properties),
                Exception = JsonConvert.DeserializeObject<Exception>(p.Exception).ToBsonDocument(),
            }), modelCollector);
        }

        public static (IEnumerable<MongoDbLogModel> logs, LogModelPropsCollector collector) Logs() => Logs(20);
    }
}
