using Ardalis.GuardClauses;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Serilog.Sinks.MariaDB;
using Serilog.Sinks.MariaDB.Extensions;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.Core;
using Serilog.Ui.MySqlProvider;
using Testcontainers.MariaDb;
using Xunit;

namespace MySql.Tests.Util;

[CollectionDefinition(nameof(MariaDbDataProvider))]
public class MariaDbCollection : ICollectionFixture<MariaDbTestProvider>
{
}

public sealed class MariaDbTestProvider : DatabaseInstance
{
    protected override string Name => nameof(MariaDbDataProvider);

    public MariaDbTestProvider()
    {
        Container = new MariaDbBuilder().Build();
    }

    public RelationalDbOptions DbOptions { get; set; } = new()
    {
        TableName = "Logs",
        Schema = "dbo"
    };

    protected override async Task CheckDbReadinessAsync()
    {
        Guard.Against.Null(Container);

        DbOptions.ConnectionString = (Container as MariaDbContainer)?.GetConnectionString();

        await using var dataContext = new MySqlConnection(DbOptions.ConnectionString);

        await dataContext.ExecuteAsync("SELECT 1");
    }

    protected override Task InitializeAdditionalAsync()
    {
        var serilog = new SerilogSinkSetup(logger =>
        {
            logger.WriteTo.MariaDB(DbOptions.ConnectionString, autoCreateTable: true, options: new MariaDBSinkOptions { TimestampInUtc = true });
        });
        Collector = serilog.InitializeLogs();

        Provider = new MariaDbDataProvider(DbOptions);

        return Task.CompletedTask;
    }
}