using Mongo2Go;
using MongoDB.Driver;
using Serilog;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Core;
using Serilog.Ui.MongoDbProvider;

namespace MongoDb.Tests.Util.Builders
{
    public class MongoDbDataProviderBuilder
    {
        private const string DefaultDbName = "IntegrationTests";

        internal readonly MongoDbRunner Runner;
        internal readonly MongoDbOptions Options;
        internal readonly IMongoClient Client;
        internal readonly IMongoDatabase Database;
        internal IDataProvider? Sut;
        internal LogModelPropsCollector? Collector;

        private MongoDbDataProviderBuilder(MongoDbOptions options)
        {
            Options = options;
            (Runner, Client) = IntegrationDbGeneration.Generate(options);
            Database = Client!.GetDatabase(options.DatabaseName);

            Sut = new MongoDbDataProvider(Client, Options);
            Collector = Seed(Options.ConnectionString);
        }

        public static MongoDbDataProviderBuilder Build()
        {
            var options = new MongoDbOptions()
                .WithCollectionName("LogCollection")
                .WithDatabaseName(DefaultDbName);
            return new MongoDbDataProviderBuilder(options);
        }

        private static LogModelPropsCollector Seed(string? connectionString)
        {
            var connectionWithDbName = connectionString!.Replace("?", $"{DefaultDbName}?");

            var serilog = new SerilogSinkSetup(logger =>
            {
                logger
                    .WriteTo.MongoDBBson((sink) =>
                    {
                        sink.SetConnectionString(connectionWithDbName ?? string.Empty);
                        sink.SetCollectionName("LogCollection");
                        sink.SetCreateCappedCollection(1024, 500);
                    });
            });

            var collector = serilog.InitializeLogs();

            return collector;
        }
    }
}