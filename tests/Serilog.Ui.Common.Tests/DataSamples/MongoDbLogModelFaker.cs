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
        public static IEnumerable<MongoDbLogModel> Logs(int generationCount)
            => LogModelFaker.Logs(generationCount).Select(p => new MongoDbLogModel
            {
                Id = p.RowNo,
                Level = p.Level,
                RenderedMessage = p.Message,
                Timestamp = p.Timestamp,
                UtcTimeStamp = p.Timestamp.ToUniversalTime(),
                Properties = JsonConvert.DeserializeObject<Properties>(p.Properties),
                Exception = JsonConvert.DeserializeObject<Exception>(p.Exception).ToBsonDocument(),
            });

        public static IEnumerable<MongoDbLogModel> Logs() => Logs(20);
    }
}
