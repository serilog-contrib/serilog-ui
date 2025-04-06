using Mongo2Go;

namespace WebApp.HostedServices;

public class MongoDbService : IHostedService, IAsyncDisposable
{
    private static MongoDbRunner? _runner;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _runner ??= MongoDbRunner.StartForDebugging(
            singleNodeReplSet: true,
            additionalMongodArguments: "--quiet",
            port: 27099);

        return Task.CompletedTask;
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