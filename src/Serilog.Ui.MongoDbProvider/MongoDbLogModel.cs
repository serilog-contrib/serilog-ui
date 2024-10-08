using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Serilog.Ui.Core.Models;

namespace Serilog.Ui.MongoDbProvider
{
    [BsonIgnoreExtraElements]
    [BsonDiscriminator(RootClass = true)]
    public class MongoDbLogModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Level { get; set; } = string.Empty;

        public string RenderedMessage { get; set; } = string.Empty;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Timestamp { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UtcTimeStamp { get; set; }

        [BsonElement("Exception")]
        public BsonDocument? Exception { get; set; }

        [BsonElement("Properties")]
        public object? Properties { get; set; }

        internal LogModel ToLogModel(int rowNoStart, int index)
        {
            return new LogModel
            {
                Level = Level,
                Message = RenderedMessage,
                Timestamp = (Timestamp ?? UtcTimeStamp).ToUniversalTime(),
                Exception = GetException(Exception),
                Properties = JsonSerializer.Serialize(Properties),
                PropertyType = "json"
            }.SetRowNo(rowNoStart, index);
        }

        private static string? GetException(object? exception)
        {
            if (exception == null || IsPropertyExist(exception, "_csharpnull"))
                return null;

            var str = exception.ToJson();
            return str;
        }

        private static bool IsPropertyExist(dynamic obj, string name)
        {
            if (obj is ExpandoObject)
                return ((IDictionary<string, object>)obj).ContainsKey(name);

            return obj.GetType()?.GetProperty(name) != null;
        }
    }
}