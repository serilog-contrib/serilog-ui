using System;
using Serilog.Ui.Core.Attributes;
using Serilog.Ui.Core.Models;

namespace Serilog.Ui.MsSqlServerProvider;

/// <summary>
/// Sql Server Log Model. <br />
/// <see cref="RowNo"/>, <see cref="Level"/>, <see cref="Message"/>, <see cref="Timestamp"/>
/// columns can't be overridden and removed from the model, due to query requirements. <br />
/// To remove a field, apply <see cref="RemovedColumnAttribute"/> on it.
/// To add a field, register the property with the correct datatype on the child class and the sink.
/// </summary>
public class SqlServerLogModel : LogModel
{
    public sealed override int RowNo => base.RowNo;

    public sealed override string Level { get; set; }

    public sealed override string Message { get; set; }

    public sealed override DateTime Timestamp { get; set; }

    public override string PropertyType => "xml";
}