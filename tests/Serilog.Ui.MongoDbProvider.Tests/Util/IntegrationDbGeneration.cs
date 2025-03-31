using System;
using Mongo2Go;
using MongoDB.Driver;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.MongoDbProvider;

namespace MongoDb.Tests.Util;

public static class IntegrationDbGeneration
{
    public static (MongoDbRunner runner, IMongoClient client) Generate(MongoDbOptions options)
    {
        var runner = MongoDbRunner.Start(singleNodeReplSet: true);
        var settings = MongoClientSettings.FromConnectionString(runner.ConnectionString);
        settings.ServerSelectionTimeout = TimeSpan.FromSeconds(10);
        var client = new MongoClient(settings);
        options.WithConnectionString(runner.ConnectionString);
        return (runner, client);
    }
}