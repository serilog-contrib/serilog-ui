using System.Threading.Tasks;
using EphemeralMongo;
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

        internal IMongoRunner? Runner;
        internal MongoDbOptions Options;
        private IMongoClient? Client;
        internal IDataProvider? Sut;
        internal LogModelPropsCollector? Collector;

        private MongoDbDataProviderBuilder(MongoDbOptions options)
        {
            Options = options;
        }

        public static Task<MongoDbDataProviderBuilder> Build()
        {
            var options = new MongoDbOptions()
                .WithCollectionName("LogCollection")
                .WithDatabaseName(DefaultDbName);
            return new MongoDbDataProviderBuilder(options).Init();
        }

        private async Task<MongoDbDataProviderBuilder> Init()
        {
            (Runner, Client) = await IntegrationDbGeneration.Generate(Options);

            Sut = new MongoDbDataProvider(Client, Options);
            Collector = Seed(Options.ConnectionString);

            return this;
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