using Mongo2Go;
using MongoDB.Driver;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Core;
using Serilog.Ui.MongoDbProvider;
using System;

namespace MongoDb.Tests.Util
{
    public class BaseServiceBuilder
    {
        internal MongoDbRunner _runner;
        internal MongoDbOptions _options;
        internal IMongoClient _client;
        internal IMongoDatabase _database;
        internal IDataProvider? _sut;
        internal LogModelPropsCollector? _collector;

        public BaseServiceBuilder(MongoDbOptions options)
        {
            _options = options;
            (_runner, _client) = IntegrationDbGeneration.Generate(options);
            _database = _client.GetDatabase(options.DatabaseName);
        }
    }
}
