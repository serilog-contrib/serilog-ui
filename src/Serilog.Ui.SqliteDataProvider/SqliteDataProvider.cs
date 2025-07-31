using Ardalis.GuardClauses;
using Dapper;
using Microsoft.Data.Sqlite;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;
using Serilog.Ui.SqliteDataProvider.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Serilog.Ui.SqliteDataProvider;

public class SqliteDataProvider(SqliteDbOptions options, SqliteQueryBuilder queryBuilder) : IDataProvider
{
    internal const string SqliteProviderName = "SQLite";
    private readonly SqliteDbOptions _options = Guard.Against.Null(options);

    public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams, CancellationToken cancellationToken = default)
    {
        queryParams.ToUtcDates(); // assuming data is saved in UTC, due to UTC predictability

        var logsTask = GetLogsAsync(queryParams);
        var logCountTask = CountLogsAsync(queryParams);

        await Task.WhenAll(logsTask, logCountTask);

        return (await logsTask, await logCountTask);
    }

    public string Name => _options.GetProviderName(SqliteProviderName);

    public async Task<DashboardModel> FetchDashboardAsync(CancellationToken cancellationToken = default)
    {
        var dashboard = new DashboardModel();
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        using var connection = new SqliteConnection(_options.ConnectionString);

        // Get total logs count
        var totalQuery = $"SELECT COUNT(*) FROM {_options.TableName}";
        dashboard.TotalLogs = await connection.QueryFirstOrDefaultAsync<int>(totalQuery);

        // Get logs count by level
        var levelQuery = $"SELECT {_options.ColumnNames.Level} as Level, COUNT(*) as Count FROM {_options.TableName} GROUP BY {_options.ColumnNames.Level}";
        var levelCounts = await connection.QueryAsync<(string Level, int Count)>(levelQuery);
        dashboard.LogsByLevel = levelCounts.ToDictionary(x => x.Level ?? "Unknown", x => x.Count);

        // Get today's logs count
        var todayQuery = $"SELECT COUNT(*) FROM {_options.TableName} WHERE {_options.ColumnNames.Timestamp} >= @StartDate AND {_options.ColumnNames.Timestamp} < @EndDate";
        dashboard.TodayLogs = await connection.QueryFirstOrDefaultAsync<int>(todayQuery, new
        {
            StartDate = StringifyDate(today),
            EndDate = StringifyDate(tomorrow)
        });

        // Get today's error logs count
        var todayErrorQuery = $"SELECT COUNT(*) FROM {_options.TableName} WHERE {_options.ColumnNames.Level} = 'Error' AND {_options.ColumnNames.Timestamp} >= @StartDate AND {_options.ColumnNames.Timestamp} < @EndDate";
        dashboard.TodayErrorLogs = await connection.QueryFirstOrDefaultAsync<int>(todayErrorQuery, new
        {
            StartDate = StringifyDate(today),
            EndDate = StringifyDate(tomorrow)
        });

        return dashboard;
    }

    private async Task<IEnumerable<LogModel>> GetLogsAsync(FetchLogsQuery queryParams)
    {
        var query = queryBuilder.BuildFetchLogsQuery(_options.ColumnNames, _options.Schema, _options.TableName, queryParams);

        var rowNoStart = queryParams.Page * queryParams.Count;

        using var connection = new SqliteConnection(_options.ConnectionString);
        var queryParameters = new
        {
            Offset = rowNoStart,
            queryParams.Count,
            queryParams.Level,
            Search = queryParams.SearchCriteria != null ? $"%{queryParams.SearchCriteria}%" : null,
            StartDate = StringifyDate(queryParams.StartDate),
            EndDate = StringifyDate(queryParams.EndDate)
        };
        var logs = await connection.QueryAsync<LogModel>(query.ToString(), queryParameters);

        return logs.Select((item, i) =>
        {
            item.PropertyType = "json";

            var ts = DateTime.SpecifyKind(item.Timestamp, item.Timestamp.Kind == DateTimeKind.Unspecified ? DateTimeKind.Utc : item.Timestamp.Kind);
            item.Timestamp = ts.ToUniversalTime();

            item.SetRowNo(rowNoStart, i);
            return item;
        }).ToList();
    }

    private Task<int> CountLogsAsync(FetchLogsQuery queryParams)
    {
        var query = queryBuilder.BuildCountLogsQuery(_options.ColumnNames, _options.Schema, _options.TableName, queryParams);

        using var connection = new SqliteConnection(_options.ConnectionString);

        return connection.QueryFirstOrDefaultAsync<int>(
            query.ToString(),
            new
            {
                queryParams.Level,
                Search = queryParams.SearchCriteria != null ? $"%{queryParams.SearchCriteria}%" : null,
                StartDate = StringifyDate(queryParams.StartDate),
                EndDate = StringifyDate(queryParams.EndDate)
            });
    }

    private static string StringifyDate(DateTime? date) => date.HasValue ? date.Value.ToString("s") + ".999" : "null";
}
