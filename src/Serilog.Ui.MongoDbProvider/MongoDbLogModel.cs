using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Dynamic;

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
        public DateTime? Timestamp { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UtcTimeStamp { get; set; }

        public dynamic Exception { get; set; }

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
                Properties = Newtonsoft.Json.JsonConvert.SerializeObject(Properties),
                PropertyType = "json"
            };
        }

        private object GetException(dynamic exception)
        {
            if (exception == null || IsPropertyExist(Exception, "_csharpnull"))
                return null;

            if (exception is string)
                return exception;

            return Newtonsoft.Json.JsonConvert.SerializeObject(Exception, Formatting.Indented);
        }

        private bool IsPropertyExist(dynamic obj, string name)
        {
            if (obj is ExpandoObject)
                return ((IDictionary<string, object>)obj).ContainsKey(name);

            return obj?.GetType()?.GetProperty(name) != null;
        }
    }
}