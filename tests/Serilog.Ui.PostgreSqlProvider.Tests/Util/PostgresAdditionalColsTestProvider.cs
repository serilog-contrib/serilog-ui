﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using NpgsqlTypes;
using Serilog.Sinks.PostgreSQL;
using Serilog.Sinks.PostgreSQL.ColumnWriters;
using Serilog.Ui.Core.Attributes;
using Serilog.Ui.PostgreSqlProvider.Models;
using Xunit;

namespace Postgres.Tests.Util;

[CollectionDefinition(nameof(PostgresAdditionalColsTestProvider))]
public class PostgresAdditionalColsCollection : ICollectionFixture<PostgresAdditionalColsTestProvider>;

public sealed class PostgresAdditionalColsTestProvider : PostgresTestProvider<PostgresTestModel>
{
    protected override Dictionary<string, ColumnWriterBase>? ColumnOptions { get; } = new()
    {
        [DefaultColumnNames.RenderedMessage] = new RenderedMessageColumnWriter(),
        [DefaultColumnNames.MessageTemplate] = new MessageTemplateColumnWriter(),
        [DefaultColumnNames.Level] = new LevelColumnWriter(),
        [DefaultColumnNames.Timestamp] = new TimestampColumnWriter(),
        [nameof(PostgresTestModel.SampleBool)] = new SinglePropertyColumnWriter(nameof(PostgresTestModel.SampleBool)) { WriteMethod = PropertyWriteMethod.Raw, DbType = NpgsqlDbType.Boolean },
        [nameof(PostgresTestModel.SampleDate)] = new SinglePropertyColumnWriter(nameof(PostgresTestModel.SampleDate)){ DbType = NpgsqlDbType.Date, WriteMethod = PropertyWriteMethod.Raw },
        [nameof(PostgresTestModel.EnvironmentName)] = new SinglePropertyColumnWriter(nameof(PostgresTestModel.EnvironmentName)){ DbType = NpgsqlDbType.Varchar},
        [nameof(PostgresTestModel.EnvironmentUserName)] = new SinglePropertyColumnWriter(nameof(PostgresTestModel.EnvironmentUserName)){ DbType = NpgsqlDbType.Varchar},
    };
}

public class PostgresTestModel : PostgresLogModel
{
    public DateTime SampleDate { get; set; }

    public bool SampleBool { get; set; }

    [CodeColumn(CodeType.Json)]
    public string EnvironmentName { get; set; } = string.Empty;

    public string EnvironmentUserName { get; set; } = string.Empty;

    [JsonIgnore, RemovedColumn]
    public override string Exception { get; set; } = string.Empty;
}