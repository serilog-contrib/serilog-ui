using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.MsSqlServerProvider;
using Testcontainers.MsSql;
using Xunit;

namespace MsSql.Tests.Util;

[CollectionDefinition(nameof(MsSqlServerTestProvider))]
public class SqlServerCollection : ICollectionFixture<MsSqlServerTestProvider>
{
}

public sealed class MsSqlServerTestProvider : MsSqlServerTestProvider<SqlServerLogModel>;

public class MsSqlServerTestProvider<T> : DatabaseInstance
    where T : SqlServerLogModel
{
    protected MsSqlServerTestProvider()
    {
        Container = new MsSqlBuilder().Build();
    }

    private RelationalDbOptions DbOptions { get; set; } = new RelationalDbOptions("dbo").WithTable("Logs");
    
    protected override string Name => nameof(MsSqlContainer);

    protected virtual ColumnOptions? ColumnOptions => null;

    protected override async Task CheckDbReadinessAsync()
    {
        DbOptions.WithConnectionString((Container as MsSqlContainer)?.GetConnectionString());

        await using var dataContext = new SqlConnection(DbOptions.ConnectionString);

        await dataContext.ExecuteAsync("SELECT DATABASEPROPERTYEX(N'master', 'Collation')");
    }

    protected override Task InitializeAdditionalAsync()
    {
        var serilog = new SerilogSinkSetup(logger =>
        {
            logger
                .Enrich.WithEnvironmentName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.AtLevel(LogEventLevel.Warning, p =>
                {
                    p.WithProperty(nameof(SqlServerTestModel.SampleBool), true);
                    p.WithProperty(nameof(SqlServerTestModel.SampleDate), new DateTime(2022, 01, 15, 10, 00, 00));
                })
                .WriteTo.MSSqlServer(DbOptions.ConnectionString,
                    new MSSqlServerSinkOptions
                    {
                        TableName = "Logs", AutoCreateSqlTable = true
                    },
                    columnOptions: ColumnOptions);
        });
        Collector = serilog.InitializeLogs();

        SqlMapper.AddTypeHandler(new DapperDateTimeHandler());
        Provider = new SqlServerDataProvider<T>(DbOptions);

        return Task.CompletedTask;
    }
}