using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Serilog.Ui.Core;
using Serilog.Ui.RavenDbProvider.Models;

namespace Serilog.Ui.RavenDbProvider;

/// <inheritdoc/>
public class RavenDbDataProvider : IDataProvider
{
    private readonly string _collectionName;
    private readonly IDocumentStore _documentStore;

    public RavenDbDataProvider(IDocumentStore documentStore, string collectionName)
    {
        _documentStore = documentStore;
        _collectionName = collectionName;
    }

    /// <inheritdoc/>
    public string Name => string.Join(".", "RavenDB", "LogEvents");

    /// <inheritdoc/>
    public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(
        int page,
        int count,
        string? level = null,
        string? searchCriteria = null,
        DateTime? startDate = null,
        DateTime? endDate = null
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

        var logsTask = GetLogsAsync(page - 1, count, level, searchCriteria, startDate, endDate);
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
        DateTime? endDate)
    {
        using var session = _documentStore.OpenAsyncSession();
        var query = session.Advanced.AsyncDocumentQuery<RavenDbLogModel>(collectionName: _collectionName).ToQueryable();

        GenerateWhereClause(ref query, level, searchCriteria, startDate, endDate);

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
}