using EphemeralMongo;

namespace WebApp.HostedServices;

public class MongoDbService : IHostedService, IAsyncDisposable
{
    private static IMongoRunner? _runner;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _runner ??= await MongoRunner.RunAsync(new()
        {
            UseSingleNodeReplicaSet = true,
            MongoPort = 27099
        }, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return DisposeAsync().AsTask();
    }

    public ValueTask DisposeAsync()
    {
        if (_runner == null) return default;

        _runner.Dispose();
        _runner = null;
        GC.SuppressFinalize(this);
        return default;
    }
}