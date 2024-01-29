using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;

namespace Serilog.Ui.Core
{
    /// <summary>
    /// Aggregates multiple <see cref="IDataProvider"/> into one instance.
    /// </summary>
    public class AggregateDataProvider : IDataProvider
    {
        private readonly Dictionary<string, IDataProvider> _dataProviders = new();

        /// <summary>
        /// It creates an instance of <see cref="AggregateDataProvider"/>.
        /// </summary>
        /// <param name="dataProviders">IEnumerable of providers.</param>
        /// <exception cref="ArgumentNullException">when <paramref name="dataProviders"/> is null</exception>
        /// <exception cref="ArgumentException">when <paramref name="dataProviders"/> is empty</exception>
        public AggregateDataProvider(IEnumerable<IDataProvider> dataProviders)
        {
            Guard.Against.NullOrEmpty(dataProviders, nameof(dataProviders));

            foreach (var grouped in dataProviders.GroupBy(dp => dp.Name, p => p, (k, e) => e.ToList()))
            {
                var name = grouped[0].Name;

                if (grouped.Count == 1)
                {
                    _dataProviders.Add(name, grouped[0]);
                }
                else
                {
                    // When providers with the same name are registered, we ensure uniqueness by
                    // generating a key I.e. ["MSSQL.dbo.logs", "MSSQL.dbo.logs"] =>
                    // ["MSSQL.dbo.logs[0]", "MSSQL.dbo.logs[1]"]
                    for (var i = 0; i < grouped.Count; i++)
                    {
                        var dataProvider = grouped[i];
                        _dataProviders.Add($"{name}[{i}]", dataProvider);
                    }
                }
            }

            SelectedDataProvider = _dataProviders.First().Value;
        }

        /// <summary>
        /// <inheritdoc cref="IDataProvider.Name"/>
        /// NOTE: We assume only one Aggregate provider, so the name is static.
        /// </summary>
        public string Name => nameof(AggregateDataProvider);

        /// <summary>
        /// If there is only one data provider, this is it.
        /// If there are multiple, this is the current data provider.
        /// </summary>
        private IDataProvider SelectedDataProvider { get; set; }

        /// <summary>
        /// Switch active data provider by key.
        /// </summary>
        /// <param name="key">Data provider key</param>
        public void SwitchToProvider(string key) => SelectedDataProvider = _dataProviders[key];

        /// <summary>
        /// Existing data providers keys.
        /// </summary>
        public IEnumerable<string> Keys => _dataProviders.Keys;

        /// <inheritdoc/>
        public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(
            int page,
            int count,
            string level = null,
            string searchCriteria = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            return SelectedDataProvider.FetchDataAsync(page, count, level, searchCriteria, startDate, endDate);
        }
    }
}