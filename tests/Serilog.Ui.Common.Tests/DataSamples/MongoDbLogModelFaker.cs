using System.Collections.Generic;
using System.Linq;
using Bogus;
using MongoDB.Bson;
using Newtonsoft.Json;
using Serilog.Ui.Common.Tests.FakeObjectModels;
using Serilog.Ui.MongoDbProvider;

namespace Serilog.Ui.Common.Tests.DataSamples
{
    public static class MongoDbLogModelFaker
    {
        public static (IEnumerable<MongoDbLogModel> logs, LogModelPropsCollector collector) Logs(int generationCount)
        {
            var originalLogs = LogModelFaker.Logs(generationCount)
                .ToList();

            var modelCollector = new LogModelPropsCollector(originalLogs);

            var faker = new Faker();

            var logs = originalLogs.Select(p => new MongoDbLogModel
            {
                Id = p.RowNo,
                Level = p.Level,
                RenderedMessage = p.Message,
                Timestamp = p.Timestamp,
                UtcTimeStamp = p.Timestamp.ToUniversalTime(),
                Properties = JsonConvert.DeserializeObject<Properties>(p.Properties),
                Exception = faker.System.Exception() // Serialization round-trip not possible for an exception, so we generate a new exception.
                    .ToBsonDocument()
            });

            return (logs, modelCollector);
        }

        public static (IEnumerable<MongoDbLogModel> logs, LogModelPropsCollector collector) Logs() => Logs(20);
    }
}