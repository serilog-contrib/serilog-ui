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

        string totalQuery = $"SELECT COUNT(*) FROM \"{options.Schema}\".\"{options.TableName}\"";
        string levelQuery =
            $"SELECT \"{options.ColumnNames.Level}\" as Level, COUNT(*) as Count FROM \"{options.Schema}\".\"{options.TableName}\" GROUP BY \"{options.ColumnNames.Level}\"";
        string todayQuery =
            $"SELECT COUNT(*) FROM \"{options.Schema}\".\"{options.TableName}\" WHERE \"{options.ColumnNames.Timestamp}\" >= @StartDate AND \"{options.ColumnNames.Timestamp}\" < @EndDate";
        string todayErrorQuery =
            $"SELECT COUNT(*) FROM \"{options.Schema}\".\"{options.TableName}\" WHERE \"{options.ColumnNames.Level}\" = @ErrorLevel AND \"{options.ColumnNames.Timestamp}\" >= @StartDate AND \"{options.ColumnNames.Timestamp}\" < @EndDate";

        Task<int> totalTask = ExecuteScalarAsync<int>(totalQuery);
        Task<IEnumerable<(int Level, int Count)>> levelTask =
            ExecuteQueryAsync<(int Level, int Count)>(levelQuery);
        Task<int> todayTask =
            ExecuteScalarAsync<int>(todayQuery, new { StartDate = today, EndDate = tomorrow });
        Task<int> todayErrorTask = ExecuteScalarAsync<int>(todayErrorQuery,
            new { ErrorLevel = LogLevelConverter.GetLevelValue("Error"), StartDate = today, EndDate = tomorrow });

        await Task.WhenAll(totalTask, levelTask, todayTask, todayErrorTask);

        Dictionary<string, int> logsByLevel =
            (await levelTask).ToDictionary(x => LogLevelConverter.GetLevelName(x.Level.ToString()), x => x.Count);

        return new LogStatisticModel
        {
            TotalLogs = await totalTask,
            LogsByLevel = logsByLevel,
            TodayLogs = await todayTask,
            TodayErrorLogs = await todayErrorTask
        };
    }

    private async Task<TResult> ExecuteScalarAsync<TResult>(string query, object? param = null)
        where TResult : struct
    {
        await using NpgsqlConnection connection = new(options.ConnectionString);
        return await connection.QueryFirstOrDefaultAsync<TResult>(query, param);
    }

    private async Task<IEnumerable<TResult>> ExecuteQueryAsync<TResult>(string query, object? param = null)
    {
        await using NpgsqlConnection connection = new(options.ConnectionString);
        return await connection.QueryAsync<TResult>(query, param);
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