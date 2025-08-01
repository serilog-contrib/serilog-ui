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

    public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams,
        CancellationToken cancellationToken = default)
    {
        queryParams.ToUtcDates(); // assuming data is saved in UTC, due to UTC predictability

        Task<IEnumerable<LogModel>> logsTask = GetLogsAsync(queryParams);
        Task<int> logCountTask = CountLogsAsync(queryParams);

        await Task.WhenAll(logsTask, logCountTask);

        return (await logsTask, await logCountTask);
    }

    public Task<DashboardModel> FetchDashboardAsync(CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();

    public string Name => _options.GetProviderName(SqliteProviderName);

    private async Task<IEnumerable<LogModel>> GetLogsAsync(FetchLogsQuery queryParams)
    {
        string query =
            queryBuilder.BuildFetchLogsQuery(_options.ColumnNames, _options.Schema, _options.TableName, queryParams);

        int rowNoStart = queryParams.Page * queryParams.Count;

        using SqliteConnection connection = new(_options.ConnectionString);
        var queryParameters = new
        {
            Offset = rowNoStart,
            queryParams.Count,
            queryParams.Level,
            Search = queryParams.SearchCriteria != null ? $"%{queryParams.SearchCriteria}%" : null,
            StartDate = StringifyDate(queryParams.StartDate),
            EndDate = StringifyDate(queryParams.EndDate)
        };
        IEnumerable<LogModel> logs = await connection.QueryAsync<LogModel>(query, queryParameters);

        return logs.Select((item, i) =>
        {
            item.PropertyType = "json";

            DateTime ts = DateTime.SpecifyKind(item.Timestamp,
                item.Timestamp.Kind == DateTimeKind.Unspecified ? DateTimeKind.Utc : item.Timestamp.Kind);
            item.Timestamp = ts.ToUniversalTime();

            item.SetRowNo(rowNoStart, i);
            return item;
        }).ToList();
    }

    private async Task<int> CountLogsAsync(FetchLogsQuery queryParams)
    {
        string query =
            queryBuilder.BuildCountLogsQuery(_options.ColumnNames, _options.Schema, _options.TableName, queryParams);

        using SqliteConnection connection = new(_options.ConnectionString);

        return await connection.QueryFirstOrDefaultAsync<int>(
            query,
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