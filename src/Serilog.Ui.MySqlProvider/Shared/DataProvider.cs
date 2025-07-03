using Dapper;
using MySqlConnector;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;
using Serilog.Ui.MySqlProvider.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Serilog.Ui.MySqlProvider.Shared;

public abstract class DataProvider<T>(MySqlDbOptions options, MySqlQueryBuilder<T> queryBuilder) : IDataProvider
    where T : MySqlLogModel
{
    public abstract string Name { get; }

    public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams, CancellationToken cancellationToken = default)
    {
        queryParams.ToUtcDates();

        var logsTask = GetLogsAsync(queryParams);
        var logCountTask = CountLogsAsync(queryParams);
        await Task.WhenAll(logsTask);

        return (await logsTask, await logCountTask);
    }

    private async Task<IEnumerable<LogModel>> GetLogsAsync(FetchLogsQuery queryParams)
    {
        string query = queryBuilder.BuildFetchLogsQuery(options.ColumnNames, options.Schema, options.TableName, queryParams);
        int rowNoStart = queryParams.Page * queryParams.Count;

        using MySqlConnection connection = new(options.ConnectionString);

        IEnumerable<T> logs = await connection.QueryAsync<T>(query, new
        {
            Offset = rowNoStart,
            queryParams.Count,
            queryParams.Level,
            Search = queryParams.SearchCriteria != null ? $"%{queryParams.SearchCriteria}%" : null,
            queryParams.StartDate,
            queryParams.EndDate
        });

        return logs
            .Select((item, i) =>
            {
                item.SetRowNo(rowNoStart, i);
                item.Level ??= item.LogLevel;
                // both sinks save UTC but MariaDb is queried as Unspecified, MySql is queried as Local
                var ts = DateTime.SpecifyKind(item.Timestamp, item.Timestamp.Kind == DateTimeKind.Unspecified ? DateTimeKind.Utc : item.Timestamp.Kind);
                item.Timestamp = ts.ToUniversalTime();
                return item;
            })
            .ToList();
    }

    private async Task<int> CountLogsAsync(FetchLogsQuery queryParams)
    {
        string query = queryBuilder.BuildCountLogsQuery(options.ColumnNames, options.Schema, options.TableName, queryParams);

        using MySqlConnection connection = new(options.ConnectionString);

        return await connection.ExecuteScalarAsync<int>(query,
            new
            {
                queryParams.Level,
                Search = queryParams.SearchCriteria != null ? "%" + queryParams.SearchCriteria + "%" : null,
                queryParams.StartDate,
                queryParams.EndDate
            });
    }

    public async Task<DashboardModel> FetchDashboardAsync(CancellationToken cancellationToken = default)
    {
        var dashboard = new DashboardModel();
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        using MySqlConnection connection = new(options.ConnectionString);

        // Get total logs count
        var totalQuery = $"SELECT COUNT(*) FROM `{options.Schema}`.`{options.TableName}`";
        dashboard.TotalLogs = await connection.QueryFirstOrDefaultAsync<int>(totalQuery);

        // Get logs count by level
        var levelQuery = $"SELECT `{options.ColumnNames.Level}` as Level, COUNT(*) as Count FROM `{options.Schema}`.`{options.TableName}` GROUP BY `{options.ColumnNames.Level}`";
        var levelCounts = await connection.QueryAsync<(string Level, int Count)>(levelQuery);
        dashboard.LogsByLevel = levelCounts.ToDictionary(x => x.Level ?? "Unknown", x => x.Count);

        // Get today's logs count
        var todayQuery = $"SELECT COUNT(*) FROM `{options.Schema}`.`{options.TableName}` WHERE `{options.ColumnNames.Timestamp}` >= @StartDate AND `{options.ColumnNames.Timestamp}` < @EndDate";
        dashboard.TodayLogs = await connection.QueryFirstOrDefaultAsync<int>(todayQuery, new
        {
            StartDate = today,
            EndDate = tomorrow
        });

        // Get today's error logs count
        var todayErrorQuery = $"SELECT COUNT(*) FROM `{options.Schema}`.`{options.TableName}` WHERE `{options.ColumnNames.Level}` = 'Error' AND `{options.ColumnNames.Timestamp}` >= @StartDate AND `{options.ColumnNames.Timestamp}` < @EndDate";
        dashboard.TodayErrorLogs = await connection.QueryFirstOrDefaultAsync<int>(todayErrorQuery, new
        {
            StartDate = today,
            EndDate = tomorrow
        });

        return dashboard;
    }
}