using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;
using Serilog.Ui.Core.OptionsBuilder;

namespace Serilog.Ui.MySqlProvider.Shared;

public abstract class DataProvider(RelationalDbOptions options) : IDataProvider
{
    protected virtual string ColumnTimestampName => "TimeStamp";

    protected virtual string ColumnLevelName => "Level";

    protected virtual string ColumnMessageName => "Message";

    protected readonly RelationalDbOptions Options = options ?? throw new ArgumentNullException(nameof(options));

    public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams, CancellationToken cancellationToken = default)
    {
        queryParams.ToUtcDates();

        var logsTask = GetLogsAsync(queryParams);
        var logCountTask = CountLogsAsync(queryParams);

        await Task.WhenAll(logsTask, logCountTask);

        return (await logsTask, await logCountTask);
    }

    public abstract string Name { get; }

    private async Task<IEnumerable<LogModel>> GetLogsAsync(FetchLogsQuery queryParams)
    {
        var queryBuilder = new StringBuilder();
        queryBuilder.Append(
            $"SELECT Id, {ColumnMessageName}, {ColumnLevelName} AS 'Level', {ColumnTimestampName}, Exception, Properties From `{Options.TableName}` ");

        GenerateWhereClause(queryBuilder, queryParams);
        var sortClause = GenerateSortClause(queryParams.SortOn, queryParams.SortBy);

        queryBuilder.Append($"ORDER BY {sortClause} LIMIT @Offset, @Count");

        var rowNoStart = queryParams.Page * queryParams.Count;

        using var connection = new MySqlConnection(Options.ConnectionString);
        var param = new
        {
            Offset = rowNoStart,
            queryParams.Count,
            queryParams.Level,
            Search = queryParams.SearchCriteria != null ? $"%{queryParams.SearchCriteria}%" : null,
            queryParams.StartDate,
            queryParams.EndDate
        };
        var logs = await connection.QueryAsync<MySqlLogModel>(queryBuilder.ToString(), param);

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

    private async Task<int> CountLogsAsync(FetchLogsQuery queryParams)
    {
        var queryBuilder = new StringBuilder();
        queryBuilder.Append($"SELECT COUNT(Id) FROM `{Options.TableName}` ");

        GenerateWhereClause(queryBuilder, queryParams);

        using var connection = new MySqlConnection(Options.ConnectionString);
        return await connection.ExecuteScalarAsync<int>(queryBuilder.ToString(),
            new
            {
                queryParams.Level,
                Search = queryParams.SearchCriteria != null ? "%" + queryParams.SearchCriteria + "%" : null,
                queryParams.StartDate,
                queryParams.EndDate
            });
    }

    private void GenerateWhereClause(StringBuilder queryBuilder, FetchLogsQuery queryParams)
    {
        var conditionStart = "WHERE";

        if (!string.IsNullOrEmpty(queryParams.Level))
        {
            queryBuilder.Append($"WHERE {ColumnLevelName} = @Level ");
            conditionStart = "AND";
        }

        if (!string.IsNullOrEmpty(queryParams.SearchCriteria))
        {
            queryBuilder.Append($"{conditionStart} ({ColumnMessageName} LIKE @Search OR Exception LIKE @Search) ");
            conditionStart = "AND";
        }

        if (queryParams.StartDate != null)
        {
            queryBuilder.Append($"{conditionStart} {ColumnTimestampName} >= @StartDate ");
            conditionStart = "AND";
        }

        if (queryParams.EndDate != null)
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