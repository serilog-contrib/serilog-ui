using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json.Serialization;
using Serilog.Sinks.MSSqlServer;
using Serilog.Ui.Common.Tests.FakeObjectModels;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.MsSqlServerProvider;
using Xunit;

namespace MsSql.Tests.Util;

[CollectionDefinition(nameof(MsSqlServerAdditionalColsTestProvider))]
public class SqlServerAdditionalColsCollection : ICollectionFixture<MsSqlServerAdditionalColsTestProvider>;

public sealed class MsSqlServerAdditionalColsTestProvider : MsSqlServerTestProvider<SqlServerTestModel>
{
    private static ColumnOptions Opts()
    {
        var cols = new ColumnOptions
        {
            AdditionalColumns = new List<SqlColumn>
            {
                new(nameof(TestLogModel.SampleDate), SqlDbType.DateTime2),
                new(nameof(TestLogModel.SampleBool), SqlDbType.Bit),
                new(nameof(TestLogModel.EnvironmentName), SqlDbType.VarChar),
                new(nameof(TestLogModel.EnvironmentUserName), SqlDbType.VarChar),
            }
        };
        cols.Store.Remove(StandardColumn.Exception);
        return cols;
    }

    protected override ColumnOptions? ColumnOptions => Opts();
}

public class SqlServerTestModel : SqlServerLogModel
{
    public DateTime SampleDate { get; set; }

    public bool SampleBool { get; set; }

    [CodeColumn(CodeType.Json)]
    public string EnvironmentName { get; set; } = string.Empty;

    public string EnvironmentUserName { get; set; } = string.Empty;

    [JsonIgnore]
    public override string Exception { get; set; } = string.Empty;
}