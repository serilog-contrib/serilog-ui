using MongoDB.Driver;
using Serilog.Ui.Common.Tests.DataSamples;
using System.Threading.Tasks;

namespace Serilog.Ui.MongoDbProvider.Tests.Util.Builders
{
    public class MongoDbDataProviderBuilder : BaseServiceBuilder
    {
        private const string DefaultDbName = "IntegrationTests";

        internal MongoDbDataProvider _sut;
        internal IMongoCollection<MongoDbLogModel> _mongoCollection;
        internal LogModelPropsCollector? _collector;

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
            await collection.InsertManyAsync(array);
            return collector;
        }
    }
}
