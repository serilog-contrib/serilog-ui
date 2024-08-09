using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Serilog.Ui.Core.Attributes;

namespace Serilog.Ui.Core.Models;

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
        var isExcRemoved = exceptionProperty?.GetCustomAttribute<RemovedColumnAttribute>();
        if (isExcRemoved is not null)
        {
            yield return nameof(LogModel.Exception);
        }

        var propertiesProperty = typeof(T).GetProperty(nameof(LogModel.Properties));
        var isPropRemoved = propertiesProperty?.GetCustomAttribute<RemovedColumnAttribute>();
        if (isPropRemoved is not null)
        {
            yield return nameof(LogModel.Exception);
        }
    }
}