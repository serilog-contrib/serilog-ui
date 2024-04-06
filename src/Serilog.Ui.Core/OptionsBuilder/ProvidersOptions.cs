using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Serilog.Ui.Core.Attributes;

namespace Serilog.Ui.Core.OptionsBuilder;

/// <summary>
/// ProvidersOptions class.
/// </summary>
public static class ProvidersOptions
{
    private static readonly ConcurrentDictionary<string, ColumnsInfo> _additionalColumns = new();

    private static readonly HashSet<string> _disabledSortProviderNames = [];

    /// <summary>
    /// Gets the AdditionalColumns.
    /// </summary>
    /// <returns></returns>
    public static ReadOnlyDictionary<string, ColumnsInfo> AdditionalColumns => new(_additionalColumns);

    /// <summary>
    /// Gets the DisabledSortProviderNames.
    /// </summary>
    public static ReadOnlyCollection<string> DisabledSortProviderNames => _disabledSortProviderNames.ToList().AsReadOnly();

    /// <summary>
    /// Register AdditionalColumns info for <see cref="LogModel"/> provider.
    /// </summary>
    /// <param name="providerKey">The provider key. Should be the same registered in the provider.</param>
    /// <typeparam name="T"></typeparam>
    public static void RegisterType<T>(string providerKey)
        where T : LogModel
    {
        _additionalColumns.TryAdd(providerKey, ColumnsInfo.Create<T>());
    }

    /// <summary>
    /// Register a Provider Name that doesn't permit sorting by property.
    /// </summary>
    /// <param name="name"></param>
    public static void RegisterDisabledSortName(string name)
    {
        _disabledSortProviderNames.Add(name);
    }
}

/// <summary>
/// ColumnsInfo options.
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
    public IEnumerable<AdditionalColumn> AdditionalColumns { get; private set; } = Array.Empty<AdditionalColumn>();

    /// <summary>
    /// LogModel RemovedColumns list.
    /// </summary>
    public IEnumerable<string> RemovedColumns { get; private set; } = Array.Empty<string>();

    /// <summary>
    /// Creates a ColumnsInfo object, from a T param (constraint <see cref="LogModel"/>).
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
    /// Column name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Column type name.
    /// </summary>
    public string TypeName { get; set; }

    /// <summary>
    /// Column code type.
    /// </summary>
    public CodeType? CodeType { get; set; }
}