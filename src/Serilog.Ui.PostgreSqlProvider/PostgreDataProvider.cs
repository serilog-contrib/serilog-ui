using Dapper;
using Npgsql;
using Serilog.Ui.Core;
using Serilog.Ui.PostgreSqlProvider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Serilog.Ui.Core.Models.SearchOptions;

namespace Serilog.Ui.PostgreSqlProvider;

/// <inheritdoc/>
public class PostgresDataProvider(PostgreSqlDbOptions options) : IDataProvider
{
    private readonly PostgreSqlDbOptions _options = options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public string Name => _options.ToDataProviderName("NPGSQL");

    /// <inheritdoc/>
    public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(
        int page,
        int count,
        string level = null,
        string searchCriteria = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        SortProperty sortOn = SortProperty.Timestamp,
        SortDirection sortBy = SortDirection.Desc
    )
    {
        if (startDate != null && startDate.Value.Kind != DateTimeKind.Utc)
        {
            startDate = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);
        }

        if (endDate != null && endDate.Value.Kind != DateTimeKind.Utc)
        {
            endDate = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);
        }

        var logsTask = GetLogsAsync(page - 1, count, level, searchCriteria, startDate, endDate, sortOn, sortBy);
        var logCountTask = CountLogsAsync(level, searchCriteria, startDate, endDate);
        await Task.WhenAll(logsTask, logCountTask);

        return (await logsTask, await logCountTask);
    }

    private async Task<IEnumerable<LogModel>> GetLogsAsync(
        int page,
        int count,
        string level,
        string searchCriteria,
        DateTime? startDate,
        DateTime? endDate,
        SortProperty sortOn,
        SortDirection sortBy)
    {
        var query = QueryBuilder.BuildFetchLogsQuery(_options.Schema, _options.TableName, level, searchCriteria, ref startDate, ref endDate, sortOn,
            sortBy);

        await using var connection = new NpgsqlConnection(_options.ConnectionString);

        var logs = await connection.QueryAsync<PostgresLogModel>(query,
            new
            {
                Offset = page * count,
                Count = count,
                // TODO: [open point]
                // this level could be a text column, to be passed as parameter:
                // https://github.com/b00ted/serilog-sinks-postgresql/blob/ce73c7423383d91ddc3823fe350c1c71fc23bab9/Serilog.Sinks.PostgreSQL/Sinks/PostgreSQL/ColumnWriters.cs#L97
                Level = LogLevelConverter.GetLevelValue(level),
                Search = searchCriteria != null ? "%" + searchCriteria + "%" : null,
                StartDate = startDate,
                EndDate = endDate
            });

        var rowNoStart = page * count;
        return logs
            .Select((item, i) =>
            {
                item.RowNo = rowNoStart + i;
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
        var query = QueryBuilder.BuildCountLogsQuery(_options.Schema, _options.TableName, level, searchCriteria, ref startDate, ref endDate);

        await using var connection = new NpgsqlConnection(_options.ConnectionString);

        return await connection.ExecuteScalarAsync<int>(query,
            new
            {
                Level = LogLevelConverter.GetLevelValue(level),
                Search = searchCriteria != null ? "%" + searchCriteria + "%" : null,
                StartDate = startDate,
                EndDate = endDate
            });
    }
}