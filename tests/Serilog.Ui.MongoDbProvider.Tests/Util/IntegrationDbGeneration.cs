using EphemeralMongo;
using MongoDB.Driver;
using Serilog.Ui.MongoDbProvider;
using System;

namespace MongoDb.Tests.Util
{
    public static class IntegrationDbGeneration
    {
        public static (IMongoRunner runner, IMongoClient client) Generate(MongoDbOptions options)
        {
            var runner = MongoRunner.Run(new MongoRunnerOptions
            {
                UseSingleNodeReplicaSet = true,
                AdditionalArguments = "--quiet"
            });
            var settings = MongoClientSettings.FromConnectionString(runner.ConnectionString);
            settings.ServerSelectionTimeout = TimeSpan.FromSeconds(10);
            var client = new MongoClient(settings);
            options.ConnectionString = runner.ConnectionString;
            return (runner, client);
        }
    }
}
