using Nest;
using Newtonsoft.Json;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using static Serilog.Ui.Core.Models.SearchOptions;

namespace Serilog.Ui.ElasticSearchProvider
{
    public class ElasticSearchDbDataProvider(IElasticClient client, ElasticSearchDbOptions options) : IDataProvider
    {
        private readonly IElasticClient _client = client ?? throw new ArgumentNullException(nameof(client));
        private readonly ElasticSearchDbOptions _options = options ?? throw new ArgumentNullException(nameof(options));

        public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(
            int page,
            int count,
            string level = null,
            string searchCriteria = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            SortProperty sortOn = SortProperty.Timestamp,
            SortDirection sortBy = SortDirection.Desc)
        {
            return GetLogsAsync(page - 1, count, level, searchCriteria, startDate, endDate, sortOn, sortBy);
        }

        public string Name => string.Join(".", "ES", _options.IndexName);

        private async Task<(IEnumerable<LogModel>, int)> GetLogsAsync(
            int page,
            int count,
            string level,
            string searchCriteria,
            DateTime? startDate = null,
            DateTime? endDate = null,
            SortProperty sortOn = SortProperty.Timestamp,
            SortDirection sortBy = SortDirection.Desc,
            CancellationToken cancellationToken = default)
        {
            // get the PropertyInfo for the Sorted property
            var sortProperty = typeof(ElasticSearchDbLogModel).GetProperty(sortOn.ToString());
            // get the actual PropertyName used by Elastic, that was set in the JsonAttribute
            var jsonAttrName = sortProperty.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;

            var descriptor = new SearchDescriptor<ElasticSearchDbLogModel>()
                .Index(_options.IndexName)
                .Query(q =>
                    +q.Match(m => m.Field(f => f.Level).Query(level)) &&
                    +q.DateRange(dr => dr.Field(f => f.Timestamp).GreaterThanOrEquals(startDate).LessThanOrEquals(endDate)) &&
                    +q.Match(m => m.Field(f => f.Message).Query(searchCriteria)) ||
                    +q.Match(m => m.Field(f => f.Exceptions).Query(searchCriteria)))
                .Sort(m => m.Field(
                    // to manage Text fields (pass throught .keyword)
                    sortProperty.PropertyType == typeof(string) ? $"{jsonAttrName}.keyword" : jsonAttrName,
                    sortBy == SortDirection.Desc ? SortOrder.Descending : SortOrder.Ascending))
                .Skip(page * count)
                .Size(count);

            var result = await _client.SearchAsync<ElasticSearchDbLogModel>(descriptor, cancellationToken);

            int.TryParse(result?.Total.ToString(), out var total);

            return (result?.Documents.Select((x, index) => x.ToLogModel(index)).ToList(), total);
        }
    }
}