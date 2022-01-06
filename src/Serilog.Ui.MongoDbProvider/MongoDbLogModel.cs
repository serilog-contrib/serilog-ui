using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Serilog.Ui.MongoDbProvider
{
    [BsonIgnoreExtraElements]
    [BsonDiscriminator(RootClass = true)]
    public class MongoDbLogModel
    {
        [BsonIgnore]
        public int Id { get; set; }

        public string Level { get; set; }

        public string RenderedMessage { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Timestamp { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UtcTimeStamp { get; set; }

        [BsonElement("Exception")]
        public BsonDocument Exception { get; set; }

        [BsonElement("Properties")]
        public object Properties { get; set; }

        internal LogModel ToLogModel()
        {
            return new LogModel
            {
                RowNo = Id,
                Level = Level,
                Message = RenderedMessage,
                Timestamp = Timestamp ?? UtcTimeStamp,
                Exception = GetException(Exception),
                Properties = JsonConvert.SerializeObject(Properties),
                PropertyType = "json"
            };
        }

        private string GetException(object exception)
        {
            if (exception == null || IsPropertyExist(exception, "_csharpnull"))
                return null;

            var str = exception.ToJson();
            return str;
        }

        private bool IsPropertyExist(dynamic obj, string name)
        {
            if (obj is ExpandoObject)
                return ((IDictionary<string, object>)obj).ContainsKey(name);

            return obj?.GetType()?.GetProperty(name) != null;
        }
    }
}