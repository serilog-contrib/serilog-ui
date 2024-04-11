using System;
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
    private string _level;

    /// <inheritdoc />
    public sealed override int RowNo => base.RowNo;

    /// <inheritdoc />
    public sealed override string Message { get; set; }

    /// <inheritdoc />
    public sealed override DateTime Timestamp { get; set; }

    /// <inheritdoc />
    public sealed override string Level
    {
        get => _level;
        set => _level = LogLevelConverter.GetLevelName(value);
    }

    /// <inheritdoc />
    public override string PropertyType => "json";
}