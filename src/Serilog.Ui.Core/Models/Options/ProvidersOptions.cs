using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Serilog.Ui.Core.Models.Options;

/// <summary>
/// ProvidersOptions class.
/// </summary>
public class ProvidersOptions
{
    private readonly ConcurrentDictionary<string, ColumnsInfo> _additionalColumns = new();

    private readonly HashSet<string> _disabledSortProviderNames = [];

    private readonly HashSet<string> _exceptionAsStringProviderNames = [];

    /// <summary>
    /// Gets the AdditionalColumns.
    /// </summary>
    /// <returns></returns>
    public ReadOnlyDictionary<string, ColumnsInfo> AdditionalColumns => new(_additionalColumns);

    /// <summary>
    /// Gets the DisabledSortProviderNames.
    /// </summary>
    public IEnumerable<string> DisabledSortProviderNames => _disabledSortProviderNames.ToList().AsReadOnly();

    /// <summary>
    /// Gets the ExceptionAsStringProviderNames.
    /// </summary>
    public IEnumerable<string> ExceptionAsStringProviderNames => _exceptionAsStringProviderNames.ToList().AsReadOnly();

    internal void RegisterType<T>(string providerKey)
        where T : LogModel
    {
        _additionalColumns.TryAdd(providerKey, ColumnsInfo.Create<T>());
    }

    internal void RegisterDisabledSortName(string name)
    {
        _disabledSortProviderNames.Add(name);
    }

    internal void RegisterExceptionAsStringProvider(string name)
    {
        _exceptionAsStringProviderNames.Add(name);
    }
}