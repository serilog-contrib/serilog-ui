using Nest;
using Newtonsoft.Json;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using static Serilog.Ui.Core.Models.SearchOptions;

namespace Serilog.Ui.ElasticSearchProvider;

public class ElasticSearchDbDataProvider(IElasticClient client, ElasticSearchDbOptions options) : IDataProvider
{
    private static readonly string TimeStampPropertyName = typeof(ElasticSearchDbLogModel)
            // get the PropertyInfo for the Sorted property
            .GetProperty(SortProperty.Timestamp.ToString())!
            // get the actual PropertyName used by Elastic, that was set in the JsonAttribute
            .GetCustomAttribute<JsonPropertyAttribute>().PropertyName ?? $"{SortProperty.Timestamp}";

    private readonly IElasticClient _client = client ?? throw new ArgumentNullException(nameof(client));

    private readonly ElasticSearchDbOptions _options = options ?? throw new ArgumentNullException(nameof(options));

    public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams,
        CancellationToken cancellationToken = default) => GetLogsAsync(queryParams, cancellationToken);

    public Task<DashboardModel> FetchDashboardAsync(CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();

    public string Name => _options.ProviderName;

    private async Task<(IEnumerable<LogModel>, int)> GetLogsAsync(FetchLogsQuery queryParams,
        CancellationToken cancellationToken = default)
    {
        // since serilog-sink does not have keyword indexes on level and message, we can only sort on @timestamp
        Func<SortDescriptor<ElasticSearchDbLogModel>, SortDescriptor<ElasticSearchDbLogModel>> sortDescriptor =
            queryParams.SortBy == SortDirection.Desc
                ? descriptor => descriptor.Descending(TimeStampPropertyName)
                : descriptor => descriptor.Ascending(TimeStampPropertyName);
        int rowNoStart = queryParams.Page * queryParams.Count;
        SearchDescriptor<ElasticSearchDbLogModel>? descriptor = new SearchDescriptor<ElasticSearchDbLogModel>()
            .Index(_options.IndexName)
            .Query(q =>
                (+q.Match(m => m.Field(f => f.Level).Query(queryParams.Level)) &&
                 +q.DateRange(dr =>
                     dr.Field(f => f.Timestamp).GreaterThanOrEquals(queryParams.StartDate)
                         .LessThanOrEquals(queryParams.EndDate)) &&
                 +q.Match(m => m.Field(f => f.Message).Query(queryParams.SearchCriteria))) ||
                +q.Match(m => m.Field(f => f.Exceptions).Query(queryParams.SearchCriteria)))
            .Sort(sortDescriptor)
            .Skip(rowNoStart)
            .Size(queryParams.Count);

        ISearchResponse<ElasticSearchDbLogModel>? result =
            await _client.SearchAsync<ElasticSearchDbLogModel>(descriptor, cancellationToken);

        int.TryParse(result?.Total.ToString(), out int total);

        return (result?.Documents.Select((x, index) => x.ToLogModel(rowNoStart, index)).ToList() ?? [], total);
    }
}