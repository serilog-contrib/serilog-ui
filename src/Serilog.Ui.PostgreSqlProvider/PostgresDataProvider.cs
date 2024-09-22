using Dapper;
using Npgsql;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;
using Serilog.Ui.PostgreSqlProvider.Extensions;
using Serilog.Ui.PostgreSqlProvider.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Serilog.Ui.PostgreSqlProvider;

/// <inheritdoc/>
public class PostgresDataProvider(PostgreSqlDbOptions options, PostgresQueryBuilder<PostgresLogModel> queryBuilder)
    : PostgresDataProvider<PostgresLogModel>(options, queryBuilder);

/// <inheritdoc />
public class PostgresDataProvider<T>(PostgreSqlDbOptions options, PostgresQueryBuilder<T> queryBuilder) : IDataProvider
    where T : PostgresLogModel
{
    internal const string ProviderName = "NPGSQL";

    /// <inheritdoc/>
    public string Name => options.GetProviderName(ProviderName);

    /// <inheritdoc/>
    public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams, CancellationToken cancellationToken = default)
    {
        queryParams.ToUtcDates();

        Task<IEnumerable<LogModel>> logsTask = GetLogsAsync(queryParams);
        Task<int> logCountTask = CountLogsAsync(queryParams);
        await Task.WhenAll(logsTask, logCountTask);

        return (await logsTask, await logCountTask);
    }

    private async Task<IEnumerable<LogModel>> GetLogsAsync(FetchLogsQuery queryParams)
    {
        string query = queryBuilder.BuildFetchLogsQuery(options.ColumnNames, options.Schema, options.TableName, queryParams);
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
        string query = queryBuilder.BuildCountLogsQuery(options.ColumnNames, options.Schema, options.TableName, queryParams);

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