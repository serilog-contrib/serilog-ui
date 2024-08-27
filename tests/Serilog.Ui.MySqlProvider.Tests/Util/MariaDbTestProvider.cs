using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Dapper;
using DotNet.Testcontainers.Containers;
using MySqlConnector;
using Serilog.Sinks.MariaDB;
using Serilog.Sinks.MariaDB.Extensions;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.Core.Models.Options;
using Serilog.Ui.MySqlProvider;
using Testcontainers.MariaDb;
using Xunit;

namespace MySql.Tests.Util;

[CollectionDefinition(nameof(MariaDbTestProvider))]
public class MariaDbCollection : ICollectionFixture<MariaDbTestProvider>;

public sealed class MariaDbTestProvider : MariaDbTestProvider<MySqlLogModel>;

public class MariaDbTestProvider<T> : DatabaseInstance
    where T : MySqlLogModel
{
    protected override string Name => nameof(MariaDbDataProvider);

    protected MariaDbTestProvider()
    {
        Container = new MariaDbBuilder().Build();
    }

    private RelationalDbOptions DbOptions { get; set; } = new RelationalDbOptions("dbo").WithTable("Logs");

    protected override sealed IContainer Container { get; set; }

    protected virtual Dictionary<string, string>? PropertiesToColumnsMapping => new MariaDBSinkOptions().PropertiesToColumnsMapping;

    protected override async Task CheckDbReadinessAsync()
    {
        Guard.Against.Null(Container);

        DbOptions.WithConnectionString((Container as MariaDbContainer)?.GetConnectionString()!);

        await using var dataContext = new MySqlConnection(DbOptions.ConnectionString);

        await dataContext.ExecuteAsync("SELECT 1");
    }

    protected override Task InitializeAdditionalAsync()
    {
        var serilog = new SerilogSinkSetup(logger =>
        {
            logger
                .WriteTo.MariaDB(
                    DbOptions.ConnectionString,
                    autoCreateTable: true,
                    options: new MariaDBSinkOptions
                    {
                        TimestampInUtc = true,
                        PropertiesToColumnsMapping = PropertiesToColumnsMapping
                    });
        });
        Collector = serilog.InitializeLogs();

        var custom = typeof(T) != typeof(MySqlLogModel);
        Provider = custom ? new MariaDbDataProvider<T>(DbOptions) : new MariaDbDataProvider(DbOptions);

        return Task.CompletedTask;
    }
}