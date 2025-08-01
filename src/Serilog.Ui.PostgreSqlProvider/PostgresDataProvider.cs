using Dapper;
using Npgsql;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;
using Serilog.Ui.PostgreSqlProvider.Extensions;
using Serilog.Ui.PostgreSqlProvider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Serilog.Ui.PostgreSqlProvider;

/// <inheritdoc />
public class PostgresDataProvider(PostgreSqlDbOptions options, PostgresQueryBuilder<PostgresLogModel> queryBuilder)
    : PostgresDataProvider<PostgresLogModel>(options, queryBuilder);

/// <inheritdoc />
public class PostgresDataProvider<T>(PostgreSqlDbOptions options, PostgresQueryBuilder<T> queryBuilder) : IDataProvider
    where T : PostgresLogModel
{
    internal const string ProviderName = "NPGSQL";

    /// <inheritdoc />
    public string Name => options.GetProviderName(ProviderName);

    /// <inheritdoc />
    public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams,
        CancellationToken cancellationToken = default)
    {
        queryParams.ToUtcDates();

        Task<IEnumerable<LogModel>> logsTask = GetLogsAsync(queryParams);
        Task<int> logCountTask = CountLogsAsync(queryParams);
        await Task.WhenAll(logsTask, logCountTask);

        return (await logsTask, await logCountTask);
    }

    /// <inheritdoc />
    public async Task<LogStatisticModel> FetchDashboardAsync(CancellationToken cancellationToken = default)
    {
        DateTime today = DateTime.Today;
        DateTime tomorrow = today.AddDays(1);

        await using NpgsqlConnection connection = new(options.ConnectionString);

        // Get total logs count
        string totalQuery = $"SELECT COUNT(*) FROM \"{options.Schema}\".\"{options.TableName}\"";
        int totalLogs = await connection.QueryFirstOrDefaultAsync<int>(totalQuery);

        // Get logs count by level
        string levelQuery =
            $"SELECT \"{options.ColumnNames.Level}\" as Level, COUNT(*) as Count FROM \"{options.Schema}\".\"{options.TableName}\" GROUP BY \"{options.ColumnNames.Level}\"";

        IEnumerable<(int Level, int Count)> levelCounts =
            await connection.QueryAsync<(int Level, int Count)>(levelQuery);

        Dictionary<string, int> logsByLevel =
            levelCounts.ToDictionary(x => LogLevelConverter.GetLevelName(x.Level.ToString()), x => x.Count);

        // Get today's logs count
        string todayQuery =
            $"SELECT COUNT(*) FROM \"{options.Schema}\".\"{options.TableName}\" WHERE \"{options.ColumnNames.Timestamp}\" >= @StartDate AND \"{options.ColumnNames.Timestamp}\" < @EndDate";
        int todayLogs =
            await connection.QueryFirstOrDefaultAsync<int>(todayQuery, new { StartDate = today, EndDate = tomorrow });

        // Get today's error logs count (Error level = 3 in PostgreSQL)
        string todayErrorQuery =
            $"SELECT COUNT(*) FROM \"{options.Schema}\".\"{options.TableName}\" WHERE \"{options.ColumnNames.Level}\" = @ErrorLevel AND \"{options.ColumnNames.Timestamp}\" >= @StartDate AND \"{options.ColumnNames.Timestamp}\" < @EndDate";
        int todayErrorLogs = await connection.QueryFirstOrDefaultAsync<int>(todayErrorQuery,
            new { ErrorLevel = LogLevelConverter.GetLevelValue("Error"), StartDate = today, EndDate = tomorrow });

        LogStatisticModel model = new()
        {
            TotalLogs = totalLogs,
            LogsByLevel = logsByLevel,
            TodayLogs = todayLogs,
            TodayErrorLogs = todayErrorLogs
        };

        return model;
    }

    private async Task<IEnumerable<LogModel>> GetLogsAsync(FetchLogsQuery queryParams)
    {
        string query =
            queryBuilder.BuildFetchLogsQuery(options.ColumnNames, options.Schema, options.TableName, queryParams);
        int rowNoStart = queryParams.Page * queryParams.Count;

        await using NpgsqlConnection connection = new(options.ConnectionString);

        IEnumerable<T> logs = await connection.QueryAsync<T>(query,
            new
            {
                Offset = rowNoStart,
                queryParams.Count,
                Level = LogLevelConverter.GetLevelValue(queryParams.Level),
                Search = queryParams.SearchCriteria != null ? "%" + queryParams.SearchCriteria + "%" : null,
                queryParams.StartDate,
                queryParams.EndDate
            });

        return logs
            .Select((item, i) =>
            {
                item.SetRowNo(rowNoStart, i);
                item.Properties = !string.IsNullOrWhiteSpace(item.Properties) ? item.Properties : item.LogEvent;
                return item;
            })
            .ToList();
    }

    private async Task<int> CountLogsAsync(FetchLogsQuery queryParams)
    {
        string query =
            queryBuilder.BuildCountLogsQuery(options.ColumnNames, options.Schema, options.TableName, queryParams);

        await using NpgsqlConnection connection = new(options.ConnectionString);

        return await connection.ExecuteScalarAsync<int>(query,
            new
            {
                Level = LogLevelConverter.GetLevelValue(queryParams.Level),
                Search = queryParams.SearchCriteria != null ? "%" + queryParams.SearchCriteria + "%" : null,
                queryParams.StartDate,
                queryParams.EndDate
            });
    }
}