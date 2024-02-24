using Ardalis.GuardClauses;
using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using Serilog;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.RavenDbProvider;
using Testcontainers.RavenDb;

namespace RavenDb.Tests.Util;

[CollectionDefinition(nameof(RavenDbDataProvider))]
public class RavenDbCollection : ICollectionFixture<RavenDbTestProvider>
{ }

public sealed class RavenDbTestProvider : DatabaseInstance
{
    private const string DbName = "TestDB";
    private readonly DocumentStore _documentStore;

    public RavenDbTestProvider()
    {
        _documentStore = new DocumentStore { Database = DbName };
        Container = new RavenDbBuilder().Build();
    }

    protected override string Name => nameof(RavenDbContainer);

    protected override Task CheckDbReadinessAsync()
    {
        Guard.Against.Null(Container);

        var container = Container as RavenDbContainer;
        _documentStore.Urls = new[] { (container)?.GetConnectionString() };
        //_documentStore.Urls = new[] { "http://localhost:8080" };
        _documentStore.Initialize();
        //_documentStore.Maintenance.Server.Send(new DeleteDatabasesOperation(DbName, true));
        _documentStore.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(DbName)));

        return Task.CompletedTask;
    }

    protected override Task InitializeAdditionalAsync()
    {
        var serilog = new SetupSerilog(_documentStore);
        Collector = serilog.InitializeLogs();

        //var logs = LogModelFaker.Logs(30).ToList();
        //Collector = new LogModelPropsCollector(logs);

        //Log.Logger = new LoggerConfiguration()
        //    .MinimumLevel.Verbose()
        //    .WriteTo.RavenDB(_documentStore)
        //    .CreateLogger();

        //Parallel.ForEach(logs, log =>
        //{
        //    log.Timestamp = DateTime.Now.AddSeconds();
        //    switch (log.Level)
        //    {
        //        case "Verbose":
        //            Log.Logger.Verbose(log.Message);
        //            break;

        //        case "Debug":
        //            Log.Logger.Debug(log.Message);
        //            break;

        //        case "Information":
        //            Log.Logger.Information(log.Message);
        //            break;

        //        case "Warning":
        //            Log.Logger.Warning(log.Message);
        //            break;

        //        case "Error":
        //            Log.Logger.Error(log.Message);
        //            break;

        //        case "Fatal":
        //            Log.Logger.Fatal(log.Message);
        //            break;
        //    }
        //});

        //await Log.CloseAndFlushAsync();

        Provider = new RavenDbDataProvider(_documentStore, "LogEvents");

        return Task.CompletedTask;
    }

    public sealed class SetupSerilog
    {
        public const string IndexPrefix = "logs-7x-default-";
        public const string TemplateName = "serilog-logs-7x";
        private readonly LoggerConfiguration loggerConfig;

        public SetupSerilog(IDocumentStore documentStore)
        {
            loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RavenDB(documentStore);
        }

        public LogModelPropsCollector InitializeLogs()
        {
            using var logger = loggerConfig.CreateLogger();
            return ElasticSearchLogModelFaker.Logs(logger);
        }
    }
}