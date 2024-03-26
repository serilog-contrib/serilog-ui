using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Serilog.Ui.Core;
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
    public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(
        int page,
        int count,
        string? level = null,
        string? searchCriteria = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        SortProperty sortOn = SortProperty.Timestamp,
        SortDirection sortBy = SortDirection.Desc
    )
    {
        if (startDate != null && startDate.Value.Kind != DateTimeKind.Utc)
        {
            startDate = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);
        }

        if (endDate != null && endDate.Value.Kind != DateTimeKind.Utc)
        {
            endDate = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);
        }

        var logsTask = GetLogsAsync(page - 1, count, level, searchCriteria, startDate, endDate, sortOn, sortBy);
        var logCountTask = CountLogsAsync(level, searchCriteria, startDate, endDate);
        await Task.WhenAll(logsTask, logCountTask);

        return (await logsTask, await logCountTask);
    }

    private async Task<IEnumerable<LogModel>> GetLogsAsync(
        int page,
        int count,
        string? level,
        string? searchCriteria,
        DateTime? startDate,
        DateTime? endDate,
        SortProperty sortOn,
        SortDirection sortBy)
    {
        using var session = _documentStore.OpenAsyncSession();
        var query = session.Advanced.AsyncDocumentQuery<RavenDbLogModel>(collectionName: _collectionName).ToQueryable();

        GenerateWhereClause(ref query, level, searchCriteria, startDate, endDate);
        GenerateSortClause(ref query, sortOn, sortBy);

        var logs = await query.Skip(count * page).Take(count).ToListAsync();

        var index = 1;

        return logs.Select(log => log.ToLogModel((page * count) + index++)).ToList();
    }

    private async Task<int> CountLogsAsync(
        string? level,
        string? searchCriteria,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        using var session = _documentStore.OpenAsyncSession();
        var query = session.Advanced.AsyncDocumentQuery<RavenDbLogModel>(collectionName: _collectionName).ToQueryable();

        GenerateWhereClause(ref query, level, searchCriteria, startDate, endDate);

        return await query.CountAsync();
    }

    private void GenerateWhereClause(
        ref IRavenQueryable<RavenDbLogModel> query,
        string? level,
        string? searchCriteria,
        DateTime? startDate,
        DateTime? endDate)
    {
        if (!string.IsNullOrEmpty(level))
        {
            query = query.Where(q => q.Level == level);
        }

        if (!string.IsNullOrEmpty(searchCriteria))
        {
            query = query
                .Search(q => q.RenderedMessage, searchCriteria)
                .Search(q => q.Exception, searchCriteria);
        }

        if (startDate != null)
        {
            query = query.Where(q => q.Timestamp >= startDate.Value);
        }

        if (endDate != null)
        {
            query = query.Where(q => q.Timestamp <= endDate.Value);
        }
    }

    private void GenerateSortClause(
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