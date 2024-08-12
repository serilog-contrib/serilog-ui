using EphemeralMongo;

namespace WebApp.HostedServices;

public class MongoDbService : IHostedService, IAsyncDisposable
{
    private static IMongoRunner? _runner;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _runner ??= MongoRunner.Run(new MongoRunnerOptions
        {
            UseSingleNodeReplicaSet = true,
            StandardOuputLogger = Console.WriteLine, // Default: null
            StandardErrorLogger = Console.WriteLine, // Default: null
            ConnectionTimeout = TimeSpan.FromSeconds(10), // Default: 30 seconds
            ReplicaSetSetupTimeout = TimeSpan.FromSeconds(5), // Default: 10 seconds
            AdditionalArguments = "--quiet", // Default: null
            MongoPort = 27099, // Default: random available port

            // EXPERIMENTAL - Only works on Windows and modern .NET (netcoreapp3.1, net5.0, net6.0, net7.0 and so on):
            // Ensures that all MongoDB child processes are killed when the current process is prematurely killed,
            // for instance when killed from the task manager or the IDE unit tests window. Processes are managed as a unit using
            // job objects: https://learn.microsoft.com/en-us/windows/win32/procthread/job-objects
            KillMongoProcessesWhenCurrentProcessExits = true // Default: false
        });

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