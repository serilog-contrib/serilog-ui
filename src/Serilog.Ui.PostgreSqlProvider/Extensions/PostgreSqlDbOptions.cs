﻿using Serilog.Ui.Core.OptionsBuilder;

namespace Serilog.Ui.PostgreSqlProvider;

/// <inheritdoc/> - Postgres implementation
public class PostgreSqlDbOptions : RelationalDbOptions
{
    /// <inheritdoc />
    public PostgreSqlDbOptions(string defaultSchemaName) : base(defaultSchemaName)
    {
    }

    /// <summary>
    /// It gets or sets SinkType.
    ///  The sink that used to store logs in the PostgreSQL database. This data provider supports
    ///  <a href="https://github.com/b00ted/serilog-sinks-postgresql">Serilog.Sinks.Postgresql</a> and
    ///  <a href="https://github.com/serilog-contrib/Serilog.Sinks.Postgresql.Alternative">Serilog.Sinks.Postgresql.Alternative</a> sinks.
    /// </summary>
    public PostgreSqlSinkType SinkType { get; private set; }

    /// <summary>
    /// Fluently sets SinkType.
    /// </summary>
    /// <param name="sinkType"></param>
    public PostgreSqlDbOptions WithSinkType(PostgreSqlSinkType sinkType)
    {
        SinkType = sinkType;
        return this;
    }
}