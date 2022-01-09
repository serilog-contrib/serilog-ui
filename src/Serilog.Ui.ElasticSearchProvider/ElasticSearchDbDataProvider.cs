using Nest;
using Newtonsoft.Json;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using static Serilog.Ui.Core.Models.SearchOptions;

namespace Serilog.Ui.ElasticSearchProvider
{
    public class ElasticSearchDbDataProvider : IDataProvider
    {
        private readonly IElasticClient _client;
        private readonly ElasticSearchDbOptions _options;

        public ElasticSearchDbDataProvider(IElasticClient client, ElasticSearchDbOptions options)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

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
            var sortProperty = typeof(ElasticSearchDbLogModel).GetProperty(sortOn.ToString());
            var jsonAttrName = sortProperty.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;
            var descriptor = new SearchDescriptor<ElasticSearchDbLogModel>()
                .Index(_options.IndexName)
                .Sort(m => m.Field(new Field(jsonAttrName), sortBy == SortDirection.Desc ? SortOrder.Descending : SortOrder.Ascending))
                .Size(count)
                .Skip(page * count)
                .Query(q =>
                    +q.Match(m => m.Field(f => f.Level).Query(level)) &&
                    +q.DateRange(dr => dr.Field(f => f.Timestamp).GreaterThanOrEquals(startDate).LessThanOrEquals(endDate)) &&
                    +q.Match(m => m.Field(f => f.Message).Query(searchCriteria)) ||
                    +q.Match(m => m.Field(f => f.Exceptions).Query(searchCriteria)));

            var result = await _client.SearchAsync<ElasticSearchDbLogModel>(descriptor, cancellationToken);

            int.TryParse(result?.Total.ToString(), out var total);

            return (result?.Documents.Select((x, index) => x.ToLogModel(index)).ToList(), total);
        }
    }
}