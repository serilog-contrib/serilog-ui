using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using DotNet.Testcontainers.Containers;
using Npgsql;
using Serilog;
using Serilog.Sinks.PostgreSQL.ColumnWriters;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.PostgreSqlProvider;
using Serilog.Ui.PostgreSqlProvider.Extensions;
using Serilog.Ui.PostgreSqlProvider.Models;
using Testcontainers.PostgreSql;
using Xunit;

namespace Postgres.Tests.Util;

[CollectionDefinition(nameof(PostgresTestProvider))]
public class PostgresCollection : ICollectionFixture<PostgresTestProvider>;

public sealed class PostgresTestProvider : PostgresTestProvider<PostgresLogModel>;

public class PostgresTestProvider<T> : DatabaseInstance
    where T : PostgresLogModel
{
    protected PostgresTestProvider()
    {
        Container = new PostgreSqlBuilder("postgres:15.1").Build();
    }

    protected override string Name => nameof(PostgreSqlContainer);

    private PostgreSqlDbOptions DbOptions { get; } = new PostgreSqlDbOptions("public")
        .WithTable("logs")
        .WithSinkType(PostgreSqlSinkType.SerilogSinksPostgreSQLAlternative);

    protected override sealed IContainer Container { get; set; }

    protected virtual Dictionary<string, ColumnWriterBase>? ColumnOptions => null;

    protected override async Task CheckDbReadinessAsync()
    {
        DbOptions.WithConnectionString((Container as PostgreSqlContainer)?.GetConnectionString()!);

        await using NpgsqlConnection dataContext = new(DbOptions.ConnectionString);

        await dataContext.ExecuteAsync("SELECT 1");
    }

    protected override Task InitializeAdditionalAsync()
    {
        SerilogSinkSetup serilog = new(logger =>
        {
            logger
                .WriteTo.PostgreSQL(
                    DbOptions.ConnectionString!,
                    "logs",
                    ColumnOptions,
                    schemaName: "public",
                    needAutoCreateTable: true,
                    failureCallback: exc => throw exc,
                    batchSizeLimit: 1);
        });

        Collector = serilog.InitializeLogs();

        bool custom = typeof(T) != typeof(PostgresLogModel);
        Provider = custom
            ? new PostgresDataProvider<T>(DbOptions, new PostgresQueryBuilder<T>())
            : new PostgresDataProvider(DbOptions, new PostgresQueryBuilder<PostgresLogModel>());

        return Task.CompletedTask;
    }
}