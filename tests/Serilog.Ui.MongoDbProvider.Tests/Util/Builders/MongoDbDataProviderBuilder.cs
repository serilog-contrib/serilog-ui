using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.MongoDbProvider;
using System.Threading.Tasks;

namespace MongoDb.Tests.Util.Builders
{
    public class MongoDbDataProviderBuilder : BaseServiceBuilder
    {
        private const string DefaultDbName = "IntegrationTests";

        internal IMongoCollection<MongoDbLogModel> _mongoCollection;

        protected MongoDbDataProviderBuilder(MongoDbOptions options) : base(options)
        {
            _mongoCollection = _database.GetCollection<MongoDbLogModel>(_options.CollectionName);
            _sut = new MongoDbDataProvider(_client, _options);
        }

        public static async Task<MongoDbDataProviderBuilder> Build(bool useLinq3)
        {
            var options = new MongoDbOptions() { CollectionName = "LogCollection", DatabaseName = DefaultDbName }; // , UseLinq3 = useLinq3 };
            var builder = new MongoDbDataProviderBuilder(options);
            builder._collector = await Seed(builder._mongoCollection);
            return builder;
        }

        public static async Task<LogModelPropsCollector> Seed(IMongoCollection<MongoDbLogModel> collection)
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
