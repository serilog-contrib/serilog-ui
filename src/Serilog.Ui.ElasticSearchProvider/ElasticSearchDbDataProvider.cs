using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;
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

    public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams, CancellationToken cancellationToken = default)
    {
        return GetLogsAsync(queryParams, cancellationToken);
    }

    public string Name => _options.ProviderName;

    private async Task<(IEnumerable<LogModel>, int)> GetLogsAsync(FetchLogsQuery queryParams, CancellationToken cancellationToken = default)
    {
        // since serilog-sink does not have keyword indexes on level and message, we can only sort on @timestamp
        Func<SortDescriptor<ElasticSearchDbLogModel>, SortDescriptor<ElasticSearchDbLogModel>> sortDescriptor =
            queryParams.SortBy == SortDirection.Desc
                ? descriptor => descriptor.Descending(TimeStampPropertyName)
                : descriptor => descriptor.Ascending(TimeStampPropertyName);
        var rowNoStart = queryParams.Page * queryParams.Count;
        var descriptor = new SearchDescriptor<ElasticSearchDbLogModel>()
            .Index(_options.IndexName)
            .Query(q =>
                +q.Match(m => m.Field(f => f.Level).Query(queryParams.Level)) &&
                +q.DateRange(dr => dr.Field(f => f.Timestamp).GreaterThanOrEquals(queryParams.StartDate).LessThanOrEquals(queryParams.EndDate)) &&
                +q.Match(m => m.Field(f => f.Message).Query(queryParams.SearchCriteria)) ||
                +q.Match(m => m.Field(f => f.Exceptions).Query(queryParams.SearchCriteria)))
            .Sort(sortDescriptor)
            .Skip(rowNoStart)
            .Size(queryParams.Count);

        var result = await _client.SearchAsync<ElasticSearchDbLogModel>(descriptor, cancellationToken);

        int.TryParse(result?.Total.ToString(), out var total);

        return (result?.Documents.Select((x, index) => x.ToLogModel(rowNoStart, index)).ToList() ?? [], total);
    }

    public async Task<DashboardModel> FetchDashboardAsync(CancellationToken cancellationToken = default)
    {
        var dashboard = new DashboardModel();
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        // Get total logs count
        var totalResponse = await _client.CountAsync<ElasticSearchDbLogModel>(c => c
            .Index(options.IndexName), cancellationToken);
        dashboard.TotalLogs = (int)(totalResponse?.Count ?? 0);

        // Get logs count by level
        var levelResponse = await _client.SearchAsync<ElasticSearchDbLogModel>(s => s
            .Index(options.IndexName)
            .Size(0)
            .Aggregations(aggs => aggs
                .Terms("levels", t => t.Field(f => f.Level))
            ), cancellationToken);
        
        if (levelResponse?.Aggregations?.Terms("levels") is { } levelsAgg)
        {
            dashboard.LogsByLevel = levelsAgg.Buckets.ToDictionary(
                bucket => bucket.Key.ToString() ?? "Unknown", 
                bucket => (int)bucket.DocCount);
        }

        // Get today's logs count
        var todayResponse = await _client.CountAsync<ElasticSearchDbLogModel>(c => c
            .Index(options.IndexName)
            .Query(q => q
                .DateRange(r => r.Field(f => f.Timestamp).GreaterThanOrEquals(today).LessThan(tomorrow))
            ), cancellationToken);
        dashboard.TodayLogs = (int)(todayResponse?.Count ?? 0);

        // Get today's error logs count
        var todayErrorResponse = await _client.CountAsync<ElasticSearchDbLogModel>(c => c
            .Index(options.IndexName)
            .Query(q => q
                .Bool(b => b
                    .Must(
                        m => m.Term(t => t.Field(f => f.Level).Value("Error")),
                        m => m.DateRange(r => r.Field(f => f.Timestamp).GreaterThanOrEquals(today).LessThan(tomorrow))
                    )
                )
            ), cancellationToken);
        dashboard.TodayErrorLogs = (int)(todayErrorResponse?.Count ?? 0);

        return dashboard;
    }
}