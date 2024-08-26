using System;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DotNet.Testcontainers.Containers;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core;

namespace Serilog.Ui.Common.Tests.SqlUtil;

public abstract class DatabaseInstance : IIntegrationRunner
{
    protected abstract string Name { get; }

    /// <summary>
    /// Gets or sets the TestContainers container.
    /// </summary>
    protected virtual IContainer Container { get; set; } = null!;

    /// <summary>
    /// Gets or sets the IDataProvider.
    /// </summary>
    protected IDataProvider? Provider { get; set; }

    protected LogModelPropsCollector? Collector { get; set; }

    public IDataProvider GetDataProvider() => Guard.Against.Null(Provider);

    public LogModelPropsCollector GetPropsCollector() => Guard.Against.Null(Collector);

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
        await CheckDbReadinessAsync();
        await InitializeAdditionalAsync();
    }

    /// <summary>
    /// Register an operation to check for db readiness.
    /// You can access the container connection string from this point onwards.
    /// Runs before <see cref="InitializeAdditionalAsync"/>.
    /// </summary>
    /// <returns><see cref="Task"/></returns>
    protected abstract Task CheckDbReadinessAsync();

    /// <summary>
    /// Register operations to set up the database in a functional status.
    /// Runs after <see cref="CheckDbReadinessAsync"/>.
    /// </summary>
    /// <returns><see cref="Task"/></returns>
    protected abstract Task InitializeAdditionalAsync();

    /// <inheritdoc/>
    public async Task DisposeAsync()
    {
        await Container.DisposeAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Dispose any additional managed dependencies.
    /// </summary>
    /// <param name="disposing">If it's disposing.</param>
    protected virtual void Dispose(bool disposing)
    {
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}