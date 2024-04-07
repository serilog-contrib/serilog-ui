using Ardalis.GuardClauses;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Serilog.Sinks.MariaDB;
using Serilog.Sinks.MariaDB.Extensions;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.MySqlProvider;
using Testcontainers.MariaDb;
using Xunit;
using System.Collections.Generic;
using Serilog;
using Serilog.Events;
using System;

namespace MySql.Tests.Util;

[CollectionDefinition(nameof(MariaDbTestProvider))]
public class MariaDbCollection : ICollectionFixture<MariaDbTestProvider>
{
}

public sealed class MariaDbTestProvider : MariaDbTestProvider<MySqlLogModel>;

public class MariaDbTestProvider<T> : DatabaseInstance
    where T : MySqlLogModel
{
    protected override string Name => nameof(MariaDbDataProvider);

    public MariaDbTestProvider()
    {
        Container = new MariaDbBuilder().Build();
    }

    public RelationalDbOptions DbOptions { get; set; } = new RelationalDbOptions("dbo").WithTable("Logs");

    protected virtual Dictionary<string, string>? PropertiesToColumnsMapping => new MariaDBSinkOptions().PropertiesToColumnsMapping;

    protected override async Task CheckDbReadinessAsync()
    {
        Guard.Against.Null(Container);

        DbOptions.WithConnectionString((Container as MariaDbContainer)?.GetConnectionString());

        await using var dataContext = new MySqlConnection(DbOptions.ConnectionString);

        await dataContext.ExecuteAsync("SELECT 1");
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
                    p.WithProperty(nameof(MariaDbTestModel.SampleBool), true);
                    p.WithProperty(nameof(MariaDbTestModel.SampleDate), new DateTime(2022, 01, 15, 10, 00, 00));
                })
                .WriteTo.MariaDB(
                    DbOptions.ConnectionString,
                    autoCreateTable: true,
                    options: new MariaDBSinkOptions
                    {
                        TimestampInUtc = true,
                        PropertiesToColumnsMapping = PropertiesToColumnsMapping
                    }
                    );
        });
        Collector = serilog.InitializeLogs();

        var custom = typeof(T) != typeof(MySqlLogModel);
        Provider = custom ? new MariaDbDataProvider<T>(DbOptions) : new MariaDbDataProvider(DbOptions);

        return Task.CompletedTask;
    }
}