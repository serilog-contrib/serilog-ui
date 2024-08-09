using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core.Models;

namespace Serilog.Ui.Core.Interfaces;

/// <summary>
/// Serilog UI options builder
/// </summary>
public interface ISerilogUiOptionsBuilder
{
    /// <summary>
    /// Gets the services collection.
    /// </summary>
    /// <value>The services.</value>
    IServiceCollection Services { get; }

    /// <summary>
    /// Register a Provider Key Name that doesn't permit sort by <see cref="SearchOptions.SortProperty"/>.
    /// </summary>
    /// <param name="providerKey">The IDataProvider <see cref="IDataProvider.Name"/>.</param>
    void RegisterDisabledSortForProviderKey(string providerKey);

    /// <summary>
    /// Register <see cref="ColumnsInfo"/> for a <see cref="LogModel"/> implementation.
    /// </summary>
    /// <param name="providerKey">The IDataProvider <see cref="IDataProvider.Name"/>.</param>
    /// <typeparam name="T"></typeparam>
    void RegisterColumnsInfo<T>(string providerKey) where T : LogModel;
}