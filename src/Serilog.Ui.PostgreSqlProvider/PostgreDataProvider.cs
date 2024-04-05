using Dapper;
using Npgsql;
using Serilog.Ui.Core;
using Serilog.Ui.PostgreSqlProvider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Serilog.Ui.Core.Models;

namespace Serilog.Ui.PostgreSqlProvider;

/// <inheritdoc/>
public class PostgresDataProvider(PostgreSqlDbOptions options) : IDataProvider
{
    private readonly PostgreSqlDbOptions _options = options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public string Name => _options.ToDataProviderName("NPGSQL");

    /// <inheritdoc/>
    public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams, CancellationToken cancellationToken = default)
    {
        queryParams.ToUtcDates();

        var logsTask = GetLogsAsync(queryParams);
        var logCountTask = CountLogsAsync(queryParams);
        await Task.WhenAll(logsTask, logCountTask);

        return (await logsTask, await logCountTask);
    }

    private async Task<IEnumerable<LogModel>> GetLogsAsync(FetchLogsQuery queryParams)
    {
        var query = QueryBuilder.BuildFetchLogsQuery(_options.Schema, _options.TableName, queryParams);
        var rowNoStart = queryParams.Page * queryParams.Count;

        await using var connection = new NpgsqlConnection(_options.ConnectionString);

        var logs = await connection.QueryAsync<PostgresLogModel>(query,
            new
            {
                Offset = rowNoStart,
                queryParams.Count,
                // TODO: [open point]
                // this level could be a text column, to be passed as parameter:
                // https://github.com/b00ted/serilog-sinks-postgresql/blob/ce73c7423383d91ddc3823fe350c1c71fc23bab9/Serilog.Sinks.PostgreSQL/Sinks/PostgreSQL/ColumnWriters.cs#L97
                Level = LogLevelConverter.GetLevelValue(queryParams.Level),
                Search = queryParams.SearchCriteria != null ? "%" + queryParams.SearchCriteria + "%" : null,
                queryParams.StartDate,
                queryParams.EndDate
            });

        return logs
            .Select((item, i) =>
            {
                item.SetRowNo(rowNoStart, i);
                return item;
            })
            .ToList();
    }

    private async Task<int> CountLogsAsync(FetchLogsQuery queryParams)
    {
        var query = QueryBuilder.BuildCountLogsQuery(_options.Schema, _options.TableName, queryParams);

        await using var connection = new NpgsqlConnection(_options.ConnectionString);

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