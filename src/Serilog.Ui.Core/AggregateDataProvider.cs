using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Serilog.Ui.Core.Services
{
    /// <summary>
    /// Aggregates multiple <see cref="IDataProvider"/> into one instance.
    /// </summary>
    public class AggregateDataProvider : IDataProvider
    {
        /// <summary>
        /// <inheritdoc cref="IDataProvider.Name"/>
        /// NOTE We assume only one Aggregate provider, so the name is static.
        /// </summary>
        public string Name => nameof(AggregateDataProvider);

        private IDataProvider _dataProvider;

        /// <summary>
        /// If there is only one data provider, this is it.
        /// If there are multiple, this is the current data provider.
        /// </summary>
        public IDataProvider DataProvider => _dataProvider;

        private readonly Dictionary<string, IDataProvider> _dataProviders = new Dictionary<string, IDataProvider>();
        
        public AggregateDataProvider(IEnumerable<IDataProvider> dataProviders)
        {
            if (dataProviders == null) throw new ArgumentNullException(nameof(dataProviders));

            foreach (var grouped in dataProviders
                         .GroupBy(c => c.Name, e => e, (k, e) => e.ToList()))
            {
                var name = grouped[0].Name;

                if (grouped.Count == 1)
                {
                    _dataProviders.Add(name, grouped[0]);
                }
                else
                {
                    // When providers with the same name are registered, we ensure uniqueness by generating a key
                    // I.e. ["MSSQL.dbo.logs", "MSSQL.dbo.logs"] => ["MSSQL.dbo.logs[0]", "MSSQL.dbo.logs[1]"]
                    for (var i = 0; i < grouped.Count; i++)
                    {
                        var dataProvider = grouped[i];
                        _dataProviders.Add($"{name}[{i}]", dataProvider);
                    }
                }
            }

            _dataProvider = _dataProviders.First(c => true).Value;
        }

        public void SwitchToProvider(string key)
            => _dataProvider = _dataProviders[key];

        public IEnumerable<string> Keys => _dataProviders.Keys;

        #region Delegating members of IDataProvider

        public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(int page, int count, string level = null, string searchCriteria = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            return await DataProvider.FetchDataAsync(page, count, level, searchCriteria, startDate, endDate);
        }

        #endregion

    }
}