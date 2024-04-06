using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Serilog.Ui.Core.Attributes;
using Serilog.Ui.Core.Models;

namespace Serilog.Ui.Core.OptionsBuilder;

/// <summary>
/// ProvidersOptions class.
/// </summary>
public class ProvidersOptions
{
    private readonly ConcurrentDictionary<string, ColumnsInfo> _additionalColumns = new();

    private readonly HashSet<string> _disabledSortProviderNames = [];

    /// <summary>
    /// Gets the AdditionalColumns.
    /// </summary>
    /// <returns></returns>
    public ReadOnlyDictionary<string, ColumnsInfo> AdditionalColumns => new(_additionalColumns);

    /// <summary>
    /// Gets the DisabledSortProviderNames.
    /// </summary>
    public IEnumerable<string> DisabledSortProviderNames => _disabledSortProviderNames.ToList().AsReadOnly();

    internal void RegisterType<T>(string providerKey)
        where T : LogModel
    {
        _additionalColumns.TryAdd(providerKey, ColumnsInfo.Create<T>());
    }

    internal void RegisterDisabledSortName(string name)
    {
        _disabledSortProviderNames.Add(name);
    }
}

/// <summary>
/// ColumnsInfo.
/// </summary>
public class ColumnsInfo
{
    private static readonly IEnumerable<string> DefaultLogModelProperties = typeof(LogModel).GetProperties().Select(prop => prop.Name);

    private ColumnsInfo()
    {
    }

    /// <summary>
    ///  AdditionalColumns info.
    /// </summary>
    public IEnumerable<AdditionalColumn> AdditionalColumns { get; private set; } = [];

    /// <summary>
    /// RemovedColumns info.
    /// </summary>
    public IEnumerable<string> RemovedColumns { get; private set; } = [];

    /// <summary>
    /// Get an instance of <see cref="ColumnsInfo"/>.
    /// </summary>
    public static ColumnsInfo Create<T>()
        where T : LogModel
        => new()
        {
            AdditionalColumns = RegisterAdditionalColumns<T>().ToList(),
            RemovedColumns = RegisterRemovedColumns<T>().ToList()
        };

    private static IEnumerable<AdditionalColumn> RegisterAdditionalColumns<T>() where T : LogModel
    {
        var additionalProps = typeof(T).GetProperties().Where(p => !DefaultLogModelProperties.Contains(p.Name));

        foreach (var additionalProp in additionalProps)
        {
            var extraAttribute = additionalProp.GetCustomAttribute<CodeColumnAttribute>();
            yield return new AdditionalColumn
            {
                Name = additionalProp.Name,
                TypeName = additionalProp.PropertyType.Name.ToLowerInvariant(),
                CodeType = extraAttribute?.CodeType
            };
        }
    }

    private static IEnumerable<string> RegisterRemovedColumns<T>()
        where T : LogModel
    {
        var exceptionProperty = typeof(T).GetProperty(nameof(LogModel.Exception));
        var isExcRemoved = exceptionProperty?.GetCustomAttribute<JsonIgnoreAttribute>();
        if (isExcRemoved is not null)
        {
            yield return nameof(LogModel.Exception);
        }

        var propertiesProperty = typeof(T).GetProperty(nameof(LogModel.Properties));
        var isPropRemoved = propertiesProperty?.GetCustomAttribute<JsonIgnoreAttribute>();
        if (isPropRemoved is not null)
        {
            yield return nameof(LogModel.Exception);
        }
    }
}

/// <summary>
/// AdditionalColumn class.
/// </summary>
public class AdditionalColumn
{
    /// <summary>
    /// Gets or sets the Name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the TypeName.
    /// </summary>
    public string TypeName { get; set; }

    /// <summary>
    /// Gets or sets the CodeType.
    /// </summary>
    public CodeType? CodeType { get; set; }
}