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

        protected MongoDbDataProviderBuilder(MongoDbOptions options) : base(options)
        {
            _mongoCollection = _database.GetCollection<MongoDbLogModel>(_options.CollectionName);
            _sut = new MongoDbDataProvider(_client, _options);
        }

        public static async Task<MongoDbDataProviderBuilder> Build(bool useLinq3)
        {
            var options = new MongoDbOptions() { CollectionName = "LogCollection", DatabaseName = DefaultDbName }; // , UseLinq3 = useLinq3 };
            var builder = new MongoDbDataProviderBuilder(options);
            await Seed(builder._mongoCollection);
            return builder;
        }

        public static Task Seed(IMongoCollection<MongoDbLogModel> collection)
        {
            var array = MongoDbLogModelFaker.Logs();
            return collection.InsertManyAsync(array);
        }
    }
}
