using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;
using Serilog.Ui.RavenDbProvider.Models;
using static Serilog.Ui.Core.Models.SearchOptions;

namespace Serilog.Ui.RavenDbProvider;

/// <inheritdoc/>
public class RavenDbDataProvider(IDocumentStore documentStore, string collectionName) : IDataProvider
{
    private readonly IDocumentStore _documentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));

    private readonly string _collectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));

    /// <inheritdoc/>
    public string Name => string.Join(".", "RavenDB");

    /// <inheritdoc/>
    public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams, CancellationToken cancellationToken = default)
    {
        queryParams.ToUtcDates();

        var logsTask = GetLogsAsync(queryParams, cancellationToken);
        var logCountTask = CountLogsAsync(queryParams, cancellationToken);
        await Task.WhenAll(logsTask, logCountTask);

        return (await logsTask, await logCountTask);
    }

    private async Task<IEnumerable<LogModel>> GetLogsAsync(FetchLogsQuery queryParams, CancellationToken cancellationToken = default)
    {
        using var session = _documentStore.OpenAsyncSession();
        var query = session.Advanced.AsyncDocumentQuery<RavenDbLogModel>(collectionName: _collectionName).ToQueryable();

        GenerateWhereClause(ref query, queryParams);
        GenerateSortClause(ref query, queryParams.SortOn, queryParams.SortBy);
        var skipStart = queryParams.Count * queryParams.Page;

        var logs = await query.Skip(skipStart).Take(queryParams.Count).ToListAsync(cancellationToken);

        return logs.Select((log, index) => log.ToLogModel(skipStart, index)).ToList();
    }

    private async Task<int> CountLogsAsync(FetchLogsQuery queryParams, CancellationToken cancellationToken = default)
    {
        using var session = _documentStore.OpenAsyncSession();
        var query = session.Advanced.AsyncDocumentQuery<RavenDbLogModel>(collectionName: _collectionName).ToQueryable();

        GenerateWhereClause(ref query, queryParams);

        return await query.CountAsync(token: cancellationToken);
    }

    private static void GenerateWhereClause(
        ref IRavenQueryable<RavenDbLogModel> query,
        FetchLogsQuery queryParams)
    {
        if (!string.IsNullOrWhiteSpace(queryParams.Level))
        {
            query = query.Where(q => q.Level == queryParams.Level);
        }

        if (!string.IsNullOrWhiteSpace(queryParams.SearchCriteria))
        {
            query = query
                .Search(q => q.RenderedMessage, queryParams.SearchCriteria)
                .Search(q => q.Exception, queryParams.SearchCriteria);
        }

        if (queryParams.StartDate != null)
        {
            query = query.Where(q => q.Timestamp >= queryParams.StartDate.Value);
        }

        if (queryParams.EndDate != null)
        {
            query = query.Where(q => q.Timestamp <= queryParams.EndDate.Value);
        }
    }

    private static void GenerateSortClause(
        ref IRavenQueryable<RavenDbLogModel> query,
        SortProperty sortOn,
        SortDirection sortBy
    )
    {
        if (sortBy == SortDirection.Asc)
        {
            query = sortOn switch
            {
                SortProperty.Level => query.OrderBy(q => q.Level),
                SortProperty.Message => query.OrderBy(q => q.RenderedMessage),
                _ => query.OrderBy(q => q.Timestamp),
            };
            return;
        }

        query = sortOn switch
        {
            SortProperty.Level => query.OrderByDescending(q => q.Level),
            SortProperty.Message => query.OrderByDescending(q => q.RenderedMessage),
            _ => query.OrderByDescending(q => q.Timestamp),
        };
    }
}