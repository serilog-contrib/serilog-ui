using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Text;
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
            var model = new LogModel();
            model.Id = Id;
            model.Level = Level;
            model.Message = RenderedMessage;
            model.Timestamp = Timestamp;
            model.Exception = Exception;
            model.Properties = JsonSerializer.Serialize(Properties);
            model.PropertyType = PropertyType;

            return model;
        }
    }
}
