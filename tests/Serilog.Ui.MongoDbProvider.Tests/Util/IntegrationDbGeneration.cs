using Mongo2Go;
using MongoDB.Driver;
using Serilog.Ui.MongoDbProvider;

namespace MongoDb.Tests.Util
{
    public static class IntegrationDbGeneration
    {
        public static (MongoDbRunner runner, IMongoClient client) Generate(MongoDbOptions options)
        {
            var runner = MongoDbRunner.Start(singleNodeReplSet: true, additionalMongodArguments: "--quiet");
            var client = new MongoClient(runner.ConnectionString);
            options.ConnectionString = runner.ConnectionString;
            return (runner, client);
        }
    }
}
