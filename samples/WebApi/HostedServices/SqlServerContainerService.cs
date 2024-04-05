using Dapper;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;

namespace WebApi.HostedServices;

public class SqlServerContainerService : IHostedService, IAsyncDisposable
{
    private static MsSqlContainer? _runner;

    internal static string? SqlConnectionString;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _runner = new MsSqlBuilder()
            .WithPortBinding(60555, 1433)
            .Build();
        await _runner.StartAsync(cancellationToken);

        await using var dataContext = new SqlConnection(_runner.GetConnectionString());
        await dataContext.ExecuteAsync("CREATE DATABASE test;");
        
        var connectionStringBuilder = new SqlConnectionStringBuilder(_runner.GetConnectionString())
        {
            InitialCatalog = "test"
        };
        SqlConnectionString = connectionStringBuilder.ToString();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return DisposeAsync().AsTask();
    }

    public async ValueTask DisposeAsync()
    {
        if (_runner == null) return;

        await _runner.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}