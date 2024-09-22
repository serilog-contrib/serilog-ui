using System;
using System.Text.Json.Serialization;
using Serilog.Ui.Core.Attributes;
using Serilog.Ui.Core.Models;

namespace Serilog.Ui.PostgreSqlProvider.Models;

/// <summary>
/// Postgres Log Model. <br />
/// <see cref="LogModel.RowNo"/>, <see cref="Level"/>, <see cref="Message"/>, <see cref="Timestamp"/>
/// columns can't be overridden and removed from the model, due to query requirements. <br />
/// To remove a field, apply <see cref="RemovedColumnAttribute"/> on it.
/// To add a field, register the property with the correct datatype on the child class and the sink.
/// </summary>
public class PostgresLogModel : LogModel
{
    private string _level = string.Empty;

    /// <inheritdoc />
    public override sealed int RowNo => base.RowNo;

    /// <inheritdoc />
    public override sealed string? Message { get; set; }

    /// <inheritdoc />
    public override sealed DateTime Timestamp { get; set; }

    /// <inheritdoc />
    public override sealed string? Level
    {
        get => _level;
        set => _level = LogLevelConverter.GetLevelName(value);
    }

    /// <summary>
    /// It gets or sets LogEventSerialized.
    /// </summary>
    [JsonIgnore]
    public string LogEvent { get; set; } = string.Empty;

    /// <inheritdoc />
    public override string PropertyType => "json";
}