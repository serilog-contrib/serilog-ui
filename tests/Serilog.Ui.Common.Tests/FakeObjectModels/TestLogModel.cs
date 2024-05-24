using System;
using System.Text.Json.Serialization;
using Serilog.Ui.Core.Attributes;
using Serilog.Ui.Core.Models;

namespace Serilog.Ui.Common.Tests.FakeObjectModels;

public class TestLogModel : LogModel
{
    public DateTime SampleDate { get; set; }

    public bool SampleBool { get; set; }

    [CodeColumn(CodeType.Json)]
    public string EnvironmentName { get; set; } = string.Empty;

    public string EnvironmentUserName { get; set; } = string.Empty;

    [JsonIgnore, RemovedColumn]
    public override string? Exception { get; set; } = string.Empty;

    [JsonIgnore, RemovedColumn]
    public override string? Properties { get; set; } = string.Empty;
}