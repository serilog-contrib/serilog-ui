using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.MongoDbProvider;

namespace MongoDb.Tests.Util.Builders
{
    public class MongoDbDataProviderBuilder : BaseServiceBuilder
    {
        private const string DefaultDbName = "IntegrationTests";

        private readonly IMongoCollection<MongoDbLogModel> _mongoCollection;

        private MongoDbDataProviderBuilder(MongoDbOptions options) : base(options)
        {
            _mongoCollection = Database.GetCollection<MongoDbLogModel>(Options.CollectionName);
            Sut = new MongoDbDataProvider(Client, Options);
        }

        public static async Task<MongoDbDataProviderBuilder> Build()
        {
            var options = new MongoDbOptions().WithCollectionName("LogCollection").WithDatabaseName(DefaultDbName);
            var builder = new MongoDbDataProviderBuilder(options);
            builder.Collector = await Seed(builder._mongoCollection);
            return builder;
        }

        private static async Task<LogModelPropsCollector> Seed(IMongoCollection<MongoDbLogModel> collection)
        {
            var (array, collector) = MongoDbLogModelFaker.Logs(100);

            // https://stackoverflow.com/a/75637412/15129749
            var objectSerializer = new ObjectSerializer(type => ObjectSerializer.DefaultAllowedTypes(type) ||
                                                                (type.FullName?.StartsWith("Serilog.Ui.Common.Tests") ?? false) ||
                                                                (type.FullName?.StartsWith("MongoDB.Bson.BsonDocument") ?? false)
            );
            BsonSerializer.RegisterSerializer(objectSerializer);

            await collection.InsertManyAsync(array);
            return collector;
        }
    }
}