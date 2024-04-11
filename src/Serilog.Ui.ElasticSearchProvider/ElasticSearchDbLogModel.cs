using System;
using System.Collections.Generic;
using Nest;
using Newtonsoft.Json;
using Serilog.Ui.Core.Models;

namespace Serilog.Ui.ElasticSearchProvider
{
    public class ElasticSearchDbLogModel
    {
        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("@timestamp")]
        [Date(Name = "@timestamp")]
        public DateTime Timestamp { get; set; }

        public Dictionary<string, object>[] Exceptions { get; set; }

        [JsonProperty("fields")]
        public Dictionary<string, object> Fields { get; set; }

        internal LogModel ToLogModel(int rowNoStart, int index)
        {
            return new LogModel
            {
                Level = Level,
                Message = Message,
                Timestamp = Timestamp.ToUniversalTime(),
                Exception = Exceptions?.Length > 0 ? JsonConvert.SerializeObject(Exceptions) : null,
                Properties = JsonConvert.SerializeObject(Fields),
                PropertyType = "json"
            }.SetRowNo(rowNoStart, index);
        }
    }
}