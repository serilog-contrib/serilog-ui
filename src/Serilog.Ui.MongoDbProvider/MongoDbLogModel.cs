using System;
using MongoDB.Bson.Serialization.Attributes;
using Serilog.Ui.Core;

namespace Serilog.Ui.MongoDbProvider
{
    [BsonIgnoreExtraElements]
    public class MongoDbLogModel
    {
        [BsonIgnore]
        public int Id { get; set; }

        public string Level { get; set; }

        public string RenderedMessage { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Timestamp { get; set; }

        public string Exception { get; set; }

        public object Properties { get; set; }

        internal LogModel ToLogModel()
        {
            return new LogModel
            {
                RowNo = Id,
                Level = Level,
                Message = RenderedMessage,
                Timestamp = Timestamp,
                Exception = Exception,
                Properties = Newtonsoft.Json.JsonConvert.SerializeObject(Properties),
                PropertyType = "json"
            };
        }
    }
}