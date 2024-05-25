using System.Text.Json.Serialization;
using Serilog.Ui.Core.Attributes;
using Serilog.Ui.MsSqlServerProvider;

namespace WebApi.Models;

internal class TestLogModel : SqlServerLogModel
{
    public DateTime SampleDate { get; set; }

    public DateTimeOffset SampleDateOffset { get; set; }

    public bool SampleBool { get; set; }

    [CodeColumn(CodeType.Json)]
    public string SampleJsonCodeColumn { get; set; } = string.Empty;

    [CodeColumn(CodeType.Xml)]
    public string SampleXmlCodeColumn { get; set; } = string.Empty;

    public string EnvironmentName { get; set; } = string.Empty;

    public string EnvironmentUserName { get; set; } = string.Empty;

    [JsonIgnore, RemovedColumn]
    public override string? Properties { get; set; } = string.Empty;
}