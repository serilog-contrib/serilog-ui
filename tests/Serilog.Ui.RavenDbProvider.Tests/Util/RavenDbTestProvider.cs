using Ardalis.GuardClauses;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using Serilog;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.RavenDbProvider;
using Serilog.Ui.RavenDbProvider.Extensions;
using Testcontainers.RavenDb;

namespace RavenDb.Tests.Util;

[CollectionDefinition(nameof(RavenDbDataProvider))]
public class RavenDbCollection : ICollectionFixture<RavenDbTestProvider>
{
}

public sealed class RavenDbTestProvider : DatabaseInstance
{
    private const string DbName = "TestDB";

    private readonly DocumentStore _documentStore;

    public RavenDbTestProvider()
    {
        _documentStore = new DocumentStore { Database = DbName };
        Container = new RavenDbBuilder()
            .WithImage("ravendb/ravendb:7.0-ubuntu-latest")
            .Build();
    }

    protected override string Name => nameof(RavenDbContainer);

    protected override async Task CheckDbReadinessAsync()
    {
        Guard.Against.Null(Container);

        var container = Container as RavenDbContainer;
        _documentStore.Urls = [container?.GetConnectionString()];
        _documentStore.Initialize();

        try
        {
            await _documentStore.Maintenance.ForDatabase(DbName).SendAsync(new GetStatisticsOperation());
        }
        catch (DatabaseDoesNotExistException)
        {
            var record = new DatabaseRecord(DbName);
            await _documentStore.Maintenance.Server.SendAsync(new CreateDatabaseOperation(record));
        }
    }

    protected override Task InitializeAdditionalAsync()
    {
        var serilog = new SerilogSinkSetup(logger =>
        {
            logger
                .WriteTo.RavenDB(_documentStore);
        });
        Collector = serilog.InitializeLogs();

        Provider = new RavenDbDataProvider(_documentStore, new RavenDbOptions().WithCollectionName("LogEvents"));

        return Task.CompletedTask;
    }
}