using System;
using System.Threading.Tasks;
using EphemeralMongo;
using MongoDB.Driver;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.MongoDbProvider;

namespace MongoDb.Tests.Util;

public static class IntegrationDbGeneration
{
    public static async Task<(IMongoRunner runner, IMongoClient client)> Generate(MongoDbOptions options)
    {
        // don't add an using here - runner should be disposed by the Generate invoker!
        var runner = await MongoRunner.RunAsync(new()
        {
            UseSingleNodeReplicaSet = true,
            MongoPort = 27098
        });
        var settings = MongoClientSettings.FromConnectionString(runner.ConnectionString);
        settings.ServerSelectionTimeout = TimeSpan.FromSeconds(10);
        var client = new MongoClient(settings);
        options.WithConnectionString(runner.ConnectionString);
        return (runner, client);
    }
}