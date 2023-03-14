using Nest;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Serilog.Ui.ElasticSearchProvider
{
    public class ElasticSearchDbDataProvider : IDataProvider
    {
        public string Name => string.Join(".", "ES", _options.IndexName);

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
            DateTime? endDate = null)
        {
            return GetLogsAsync(page - 1, count, level, searchCriteria, startDate, endDate);
        }

        private async Task<(IEnumerable<LogModel>, int)> GetLogsAsync(
            int page,
            int count,
            string level,
            string searchCriteria,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default)
        {
            var descriptor = new SearchDescriptor<ElasticSearchDbLogModel>()
                .Index(_options.IndexName)
                .Sort(m => m.Descending(f => f.Timestamp))
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