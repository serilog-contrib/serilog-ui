using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog.Ui.Core.Models;

namespace Serilog.Ui.RavenDbProvider.Models;

/// <summary>
/// Note: don't remove <see cref="Newtonsoft.Json"/> as the provider will break on Exception serialization.
/// </summary>
internal class RavenDbLogModel
{
    public DateTimeOffset Timestamp { get; set; }

    public string MessageTemplate { get; set; } = null!;

    public string Level { get; set; } = null!;

    public JObject? Exception { get; set; }

    public string RenderedMessage { get; set; } = null!;

    public IDictionary<string, object>? Properties { get; set; }

    public LogModel ToLogModel(int rowNo, int index) => new LogModel()
    {
        Id = null, // no id is registered for RavenDb
        Level = Level,
        Message = RenderedMessage,
        Timestamp = Timestamp.DateTime.ToUniversalTime(),
        Exception = Exception?.ToString(Formatting.None),
        Properties = JsonConvert.SerializeObject(Properties),
        PropertyType = "json"
    }.SetRowNo(rowNo, index);
}