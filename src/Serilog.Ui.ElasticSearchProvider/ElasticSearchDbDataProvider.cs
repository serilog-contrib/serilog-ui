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
            string searchCriteria = null)
        {
            return GetLogsAsync(page - 1, count, level, searchCriteria);
        }

        private async Task<(IEnumerable<LogModel>, int)> GetLogsAsync(
            int page,
            int count,
            string level,
            string searchCriteria,
            CancellationToken cancellationToken = default)
        {
            var descriptor = new SearchDescriptor<ElasticSearchDbLogModel>()
                .Index(_options.IndexName)
                .Sort(m => m.Descending(f => f.Timestamp))
                .Size(count)
                .Skip(page * count);

            if (!string.IsNullOrEmpty(level))
                descriptor.Query(q => q.Match(m => m.Field(f => f.Level).Query(level)));

            if (!string.IsNullOrEmpty(searchCriteria))
                descriptor.Query(q => q
                    .Match(m => m
                        .Field(f => f.Message)
                        .Query(searchCriteria)
                    ) || q
                    .Match(m => m
                        .Field(f => f.Exceptions)
                        .Query(searchCriteria)
                    )
                );

            var result = await _client.SearchAsync<ElasticSearchDbLogModel>(descriptor, cancellationToken);

            int.TryParse(result?.Total.ToString(), out var total);

            return (result?.Documents.Select((x, index) => x.ToLogModel(index)).ToList(), total);
        }
    }
}