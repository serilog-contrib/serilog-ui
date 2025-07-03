using Dapper;
using Microsoft.Data.SqlClient;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;
using Serilog.Ui.MsSqlServerProvider.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Serilog.Ui.MsSqlServerProvider;

/// <inheritdoc/>
public class SqlServerDataProvider(SqlServerDbOptions options, SqlServerQueryBuilder<SqlServerLogModel> queryBuilder)
    : SqlServerDataProvider<SqlServerLogModel>(options, queryBuilder);

/// <inheritdoc/>
public class SqlServerDataProvider<T>(SqlServerDbOptions options, SqlServerQueryBuilder<T> queryBuilder) : IDataProvider
    where T : SqlServerLogModel
{
    internal const string MsSqlProviderName = "MsSQL";

    /// <inheritdoc/>
    public string Name => options.GetProviderName(MsSqlProviderName);

    public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams, CancellationToken cancellationToken = default)
    {
        // since sink stores dates in local time, we query by local time
        queryParams.ToLocalDates();

        var logsTask = GetLogsAsync(queryParams);
        var logCountTask = CountLogsAsync(queryParams);

        await Task.WhenAll(logsTask, logCountTask);

        return (await logsTask, await logCountTask);
    }

    private async Task<IEnumerable<LogModel>> GetLogsAsync(FetchLogsQuery queryParams)
    {
        string query = queryBuilder.BuildFetchLogsQuery(options.ColumnNames, options.Schema, options.TableName, queryParams);
        int rowNoStart = queryParams.Page * queryParams.Count;

        using IDbConnection connection = new SqlConnection(options.ConnectionString);

        IEnumerable<T> logs = await connection.QueryAsync<T>(query,
            new
            {
                Offset = rowNoStart,
                queryParams.Count,
                queryParams.Level,
                Search = queryParams.SearchCriteria != null ? $"%{queryParams.SearchCriteria}%" : null,
                queryParams.StartDate,
                queryParams.EndDate
            });

        return logs.Select((item, i) => item.SetRowNo(rowNoStart, i)).ToList();
    }

    private async Task<int> CountLogsAsync(FetchLogsQuery queryParams)
    {
        string query = queryBuilder.BuildCountLogsQuery(options.ColumnNames, options.Schema, options.TableName, queryParams);

        using IDbConnection connection = new SqlConnection(options.ConnectionString);

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

        using IDbConnection connection = new SqlConnection(options.ConnectionString);

        // Get total logs count
        var totalQuery = $"SELECT COUNT(*) FROM [{options.Schema}].[{options.TableName}]";
        dashboard.TotalLogs = await connection.QueryFirstOrDefaultAsync<int>(totalQuery);

        // Get logs count by level
        var levelQuery = $"SELECT [{options.ColumnNames.Level}] as Level, COUNT(*) as Count FROM [{options.Schema}].[{options.TableName}] GROUP BY [{options.ColumnNames.Level}]";
        var levelCounts = await connection.QueryAsync<(string Level, int Count)>(levelQuery);
        dashboard.LogsByLevel = levelCounts.ToDictionary(x => x.Level ?? "Unknown", x => x.Count);

        // Get today's logs count
        var todayQuery = $"SELECT COUNT(*) FROM [{options.Schema}].[{options.TableName}] WHERE [{options.ColumnNames.Timestamp}] >= @StartDate AND [{options.ColumnNames.Timestamp}] < @EndDate";
        dashboard.TodayLogs = await connection.QueryFirstOrDefaultAsync<int>(todayQuery, new
        {
            StartDate = today,
            EndDate = tomorrow
        });

        // Get today's error logs count
        var todayErrorQuery = $"SELECT COUNT(*) FROM [{options.Schema}].[{options.TableName}] WHERE [{options.ColumnNames.Level}] = 'Error' AND [{options.ColumnNames.Timestamp}] >= @StartDate AND [{options.ColumnNames.Timestamp}] < @EndDate";
        dashboard.TodayErrorLogs = await connection.QueryFirstOrDefaultAsync<int>(todayErrorQuery, new
        {
            StartDate = today,
            EndDate = tomorrow
        });

        return dashboard;
    }
}