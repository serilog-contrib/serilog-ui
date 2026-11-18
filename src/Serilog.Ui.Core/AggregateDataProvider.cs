using Ardalis.GuardClauses;
using Serilog.Ui.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Serilog.Ui.Core;

/// <summary>
///     Aggregates multiple <see cref="IDataProvider" /> into one instance.
/// </summary>
public class AggregateDataProvider : IDataProvider
{
    private readonly Dictionary<string, IDataProvider> _dataProviders = new();

    /// <summary>
    ///     It creates an instance of <see cref="AggregateDataProvider" />.
    /// </summary>
    /// <param name="dataProviders">IEnumerable of providers.</param>
    /// <exception cref="ArgumentNullException">when <paramref name="dataProviders" /> is null</exception>
    /// <exception cref="ArgumentException">when <paramref name="dataProviders" /> is empty</exception>
    public AggregateDataProvider(IEnumerable<IDataProvider> dataProviders)
    {
        List<IDataProvider> providers = Guard.Against.NullOrEmpty(dataProviders).ToList();

        foreach (List<IDataProvider>? grouped in providers.GroupBy(dp => dp.Name, p => p, (_, e) => e.ToList()))
        {
            string name = grouped[0].Name;

            if (grouped.Count == 1)
                _dataProviders.Add(name, grouped[0]);
            else
                // When providers with the same name are registered, we ensure uniqueness by
                // generating a key I.e. ["MSSQL.dbo.logs", "MSSQL.dbo.logs"] =>
                // ["MSSQL.dbo.logs[0]", "MSSQL.dbo.logs[1]"]
                for (int i = 0; i < grouped.Count; i++)
                {
                    IDataProvider? dataProvider = grouped[i];
                    _dataProviders.Add($"{name}[{i}]", dataProvider);
                }
        }

        SelectedDataProvider = _dataProviders.First().Value;
    }

    /// <summary>
    ///     If there is only one data provider, this is it.
    ///     If there are multiple, this is the current data provider.
    /// </summary>
    private IDataProvider SelectedDataProvider { get; set; }

    /// <summary>
    ///     Existing data providers keys.
    /// </summary>
    public IEnumerable<string> Keys => _dataProviders.Keys;

    /// <summary>
    ///     <inheritdoc cref="IDataProvider.Name" />
    ///     NOTE: We assume only one Aggregate provider, so the name is static.
    /// </summary>
    public string Name => nameof(AggregateDataProvider);

    /// <inheritdoc />
    public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams,
        CancellationToken cancellationToken = default)
        => SelectedDataProvider.FetchDataAsync(queryParams, cancellationToken);

    /// <inheritdoc />
    public Task<LogStatisticModel> FetchDashboardAsync(CancellationToken cancellationToken = default)
        => SelectedDataProvider.FetchDashboardAsync(cancellationToken);

    /// <summary>
    ///     Switch active data provider by key.
    /// </summary>
    /// <param name="key">Data provider key</param>
    public void SwitchToProvider(string key) => SelectedDataProvider = _dataProviders[key];
}