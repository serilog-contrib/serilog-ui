using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Serilog.Ui.Common.Tests.FakeObjectModels
{
    internal class Properties
    {
        public string SourceContext { get; set; }

        [JsonConverter(typeof(EventIdConverter))]
        public EventId EventId { get; set; }

        public string Protocol { get; set; }

        public string Host { get; set; }
    }

    /// <summary>
    /// A custom converter for the <see cref="EventId"/> class, since constructing an immutable struct type is not supported when [JsonConstructor] is not used.
    /// See: <see href="https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/immutability"/>
    /// </summary>
    internal class EventIdConverter : JsonConverter<EventId>
    {
        public override EventId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var obj = (JsonObject) JsonNode.Parse(ref reader) ?? throw new JsonException();

            var id = (obj[nameof(EventId.Id)] ?? throw new JsonException()).GetValue<int>();
            var name = (obj[nameof(EventId.Name)] ?? throw new JsonException()).GetValue<string>();
            return new EventId(id, name);
        }

        public override void Write(Utf8JsonWriter writer, EventId value, JsonSerializerOptions options)
        {
            // STJ handles writing fine, so we just delegate to the default implementation.
            ((JsonConverter<EventId>)JsonSerializerOptions.Default.GetConverter(typeof(EventId))).Write(writer, value, options);
        }
    }
}
