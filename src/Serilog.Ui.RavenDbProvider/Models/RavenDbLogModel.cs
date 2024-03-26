using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog.Ui.Core;

namespace Serilog.Ui.RavenDbProvider.Models;

internal class RavenDbLogModel
{
    public DateTimeOffset Timestamp { get; set; }

    public string MessageTemplate { get; set; } = null!;

    public string Level { get; set; } = null!;

    public JObject? Exception { get; set; }

    public string RenderedMessage { get; set; } = null!;

    public IDictionary<string, object>? Properties { get; set; }

    public LogModel ToLogModel(int rowNo) => new()
    {
        RowNo = rowNo,
        Level = Level,
        Message = RenderedMessage,
        Timestamp = Timestamp.DateTime.ToUniversalTime(),
        Exception = Exception?.ToString(Formatting.None),
        Properties = JsonConvert.SerializeObject(Properties),
        PropertyType = "json"
    };
}