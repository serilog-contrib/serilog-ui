using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Serilog.Ui.Core;
using System;
using System.Text.Json;

namespace Serilog.Ui.MongoDbServerProvider
{
    [BsonIgnoreExtraElements]
    public class MongoDbLogModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Level { get; set; }

        public string RenderedMessage { get; set; }

        public DateTime Timestamp { get; set; }

        public string Exception { get; set; }

        public object Properties { get; set; }

        public string PropertyType => "json";

        internal LogModel ToLogModel()
        {
            var model = new LogModel
            {
                Id = Id,
                Level = Level,
                Message = RenderedMessage,
                Timestamp = Timestamp,
                Exception = Exception,
                Properties = JsonSerializer.Serialize(Properties),
                PropertyType = PropertyType
            };

            return model;
        }
    }
}