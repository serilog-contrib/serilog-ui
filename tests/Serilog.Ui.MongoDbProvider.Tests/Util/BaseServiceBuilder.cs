using EphemeralMongo;
using MongoDB.Driver;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Core;
using Serilog.Ui.MongoDbProvider;

namespace MongoDb.Tests.Util
{
    public class BaseServiceBuilder
    {
        internal readonly IMongoRunner Runner;
        internal readonly MongoDbOptions Options;
        internal readonly IMongoClient Client;
        internal readonly IMongoDatabase Database;
        internal IDataProvider? Sut;
        internal LogModelPropsCollector? Collector;

        public BaseServiceBuilder(MongoDbOptions options)
        {
            Options = options;
            (Runner, Client) = IntegrationDbGeneration.Generate(options);
            Database = Client.GetDatabase(options.DatabaseName);
        }
    }
}
