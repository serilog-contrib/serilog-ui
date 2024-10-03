using Dapper;
using Microsoft.Data.SqlClient;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;
using Serilog.Ui.MsSqlServerProvider.Extensions;
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
}