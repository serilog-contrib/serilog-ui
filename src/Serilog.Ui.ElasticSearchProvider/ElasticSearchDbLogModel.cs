using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;

namespace Serilog.Ui.MongoDbProvider
{
    public class ElasticSearchDbLogModel 
    {
        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("@timestamp")]
        public DateTime Timestamp { get; set; }

        public JArray Exceptions { get; set; }

        [JsonProperty("fields")]
        public Dictionary<string, object> Fields { get; set; }

        internal LogModel ToLogModel(int index)
        {
            return new LogModel
            {
                RowNo = index,
                Level = Level,
                Message = Message,
                Timestamp = Timestamp,
                Exception = Exceptions?.Count > 0 ? BuildExceptionMessage(Exceptions[0]) : string.Empty,
                Properties = JsonConvert.SerializeObject(Fields),
                PropertyType = "json"
            };
        }

        static string BuildExceptionMessage(JToken jObjet)
        {
            return $"Exception: {jObjet.Value<string>("Message")}. StackTrace: {jObjet.Value<string>("StackTraceString")}.";
        }
    }
}