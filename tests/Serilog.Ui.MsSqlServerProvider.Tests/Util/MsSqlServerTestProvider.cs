using System.Threading.Tasks;
using Dapper;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.Data.SqlClient;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.MsSqlServerProvider;
using Serilog.Ui.MsSqlServerProvider.Extensions;
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
        // ref: https://github.com/testcontainers/testcontainers-dotnet/issues/1220#issuecomment-2247831975
        var waitStrategy = Wait
            .ForUnixContainer()
            .UntilCommandIsCompleted("/opt/mssql-tools18/bin/sqlcmd", "-C", "-Q", "SELECT 1;");
        Container = new MsSqlBuilder()
            .WithWaitStrategy(waitStrategy)
            .Build();
    }

    private SqlServerDbOptions DbOptions { get; } = new SqlServerDbOptions("dbo").WithTable("Logs");

    protected override sealed IContainer Container { get; set; }

    protected override string Name => nameof(MsSqlContainer);

    protected virtual ColumnOptions? ColumnOptions => null;

    protected override async Task CheckDbReadinessAsync()
    {
        DbOptions.WithConnectionString((Container as MsSqlContainer)?.GetConnectionString()!);

        await using var dataContext = new SqlConnection(DbOptions.ConnectionString);

        await dataContext.ExecuteAsync("SELECT DATABASEPROPERTYEX(N'master', 'Collation')");
    }

    protected override Task InitializeAdditionalAsync()
    {
        var serilog = new SerilogSinkSetup(logger =>
        {
            logger
                .WriteTo.MSSqlServer(DbOptions.ConnectionString,
                    new MSSqlServerSinkOptions
                    {
                        TableName = "Logs",
                        AutoCreateSqlTable = true
                    },
                    columnOptions: ColumnOptions);
        });
        Collector = serilog.InitializeLogs();

        SqlMapper.AddTypeHandler(new DapperDateTimeHandler());

        var custom = typeof(T) != typeof(SqlServerLogModel);
        Provider = custom
            ? new SqlServerDataProvider<T>(DbOptions, new SqlServerQueryBuilder<T>())
            : new SqlServerDataProvider(DbOptions, new SqlServerQueryBuilder<SqlServerLogModel>());

        return Task.CompletedTask;
    }
}