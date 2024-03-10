using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;

namespace Serilog.Ui.MySqlProvider.Shared;

public abstract class DataProvider(RelationalDbOptions options) : IDataProvider
{
    protected virtual string ColumnTimestampName { get; } = "TimeStamp";

    protected virtual string ColumnLevelName { get; } = "Level";

    protected virtual string ColumnMessageName { get; } = "Message";

    protected readonly RelationalDbOptions Options = options ?? throw new ArgumentNullException(nameof(options));

    public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(
        int page,
        int count,
        string level = null,
        string searchCriteria = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        SearchOptions.SortProperty sortOn = SearchOptions.SortProperty.Timestamp,
        SearchOptions.SortDirection sortBy = SearchOptions.SortDirection.Desc
    )
    {
        var logsTask = GetLogsAsync(page - 1, count, level, searchCriteria, startDate, endDate, sortOn, sortBy);
        var logCountTask = CountLogsAsync(level, searchCriteria, startDate, endDate);

        await Task.WhenAll(logsTask, logCountTask);

        return (await logsTask, await logCountTask);
    }

    public abstract string Name { get; }

    private async Task<IEnumerable<LogModel>> GetLogsAsync(
        int page,
        int count,
        string level,
        string searchCriteria,
        DateTime? startDate,
        DateTime? endDate,
        SearchOptions.SortProperty sortOn,
        SearchOptions.SortDirection sortBy)
    {
        var queryBuilder = new StringBuilder();
        queryBuilder.Append(
            $"SELECT Id, {ColumnMessageName}, {ColumnLevelName} AS 'Level', {ColumnTimestampName}, Exception, Properties From `{Options.TableName}` ");

        GenerateWhereClause(queryBuilder, level, searchCriteria, startDate, endDate);
        var sortClause = GenerateSortClause(sortOn, sortBy);

        queryBuilder.Append($"ORDER BY {sortClause} LIMIT @Offset, @Count");

        using var connection = new MySqlConnection(Options.ConnectionString);
        var param = new
        {
            Offset = page * count,
            Count = count,
            Level = level,
            Search = searchCriteria != null ? $"%{searchCriteria}%" : null,
            StartDate = startDate,
            EndDate = endDate
        };
        var logs = await connection.QueryAsync<MySqlLogModel>(queryBuilder.ToString(), param);

        var rowNoStart = page * count;
        return logs
            .Select((item, i) =>
            {
                item.RowNo = rowNoStart + i;
                // both sinks save UTC but MariaDb is queried as Unspecified, MySql is queried as Local 
                var ts = DateTime.SpecifyKind(item.Timestamp,
                    item.Timestamp.Kind == DateTimeKind.Unspecified ? DateTimeKind.Utc : item.Timestamp.Kind);
                item.Timestamp = ts.ToUniversalTime();
                return item;
            })
            .ToList();
    }

    private async Task<int> CountLogsAsync(
        string level,
        string searchCriteria,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var queryBuilder = new StringBuilder();
        queryBuilder.Append($"SELECT COUNT(Id) FROM `{Options.TableName}` ");

        GenerateWhereClause(queryBuilder, level, searchCriteria, startDate, endDate);

        using var connection = new MySqlConnection(Options.ConnectionString);
        return await connection.ExecuteScalarAsync<int>(queryBuilder.ToString(),
            new
            {
                Level = level,
                Search = searchCriteria != null ? "%" + searchCriteria + "%" : null,
                StartDate = startDate,
                EndDate = endDate
            });
    }

    private void GenerateWhereClause(
        StringBuilder queryBuilder,
        string level,
        string searchCriteria,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var conditionStart = "WHERE";

        if (!string.IsNullOrEmpty(level))
        {
            queryBuilder.Append($"WHERE {ColumnLevelName} = @Level ");
            conditionStart = "AND";
        }

        if (!string.IsNullOrEmpty(searchCriteria))
        {
            queryBuilder.Append($"{conditionStart} ({ColumnMessageName} LIKE @Search OR Exception LIKE @Search) ");
            conditionStart = "AND";
        }

        if (startDate != null)
        {
            queryBuilder.Append($"{conditionStart} {ColumnTimestampName} >= @StartDate ");
            conditionStart = "AND";
        }

        if (endDate != null)
        {
            queryBuilder.Append($"{conditionStart} {ColumnTimestampName} <= @EndDate ");
        }
    }

    private string GenerateSortClause(SearchOptions.SortProperty sortOn, SearchOptions.SortDirection sortBy)
    {
        var sortProperty = sortOn == SearchOptions.SortProperty.Level ? ColumnLevelName : sortOn.ToString();
        return $"{sortProperty} {sortBy.ToString().ToUpper()}";
    }
}