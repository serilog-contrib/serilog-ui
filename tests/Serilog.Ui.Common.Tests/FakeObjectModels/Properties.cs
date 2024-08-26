using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Serilog.Ui.Common.Tests.FakeObjectModels;

public class Properties
{
    public string SourceContext { get; set; } = string.Empty;

    [JsonConverter(typeof(EventIdConverter))]
    public EventId EventId { get; set; }

    public string Protocol { get; set; } = string.Empty;

    public string Host { get; set; } = string.Empty;
}