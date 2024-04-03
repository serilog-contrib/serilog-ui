using Dapper;
using Microsoft.Data.SqlClient;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog.Ui.Core.Models;
using Serilog.Ui.Core.OptionsBuilder;
using static Serilog.Ui.Core.Models.SearchOptions;

namespace Serilog.Ui.MsSqlServerProvider
{
    public class SqlServerDataProvider(RelationalDbOptions options) : IDataProvider
    {
        private const string ColumnTimestampName = "TimeStamp";

        private const string ColumnLevelName = "Level";

        private const string ColumnMessageName = "Message";

        private readonly RelationalDbOptions _options = options ?? throw new ArgumentNullException(nameof(options));

        public string Name => _options.ToDataProviderName("MsSQL");

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
            var queryBuilder = new StringBuilder();
            const string level = $"[{ColumnLevelName}]";
            const string message = $"[{ColumnMessageName}]";
            const string timestamp = $"[{ColumnTimestampName}]";
            queryBuilder.Append($"SELECT [Id], {message}, {level}, {timestamp}, [Exception], [Properties] ");
            queryBuilder.Append($"FROM [{_options.Schema}].[{_options.TableName}] ");

            GenerateWhereClause(queryBuilder, queryParams);

            var sortOnCol = GetColumnName(queryParams.SortOn);
            var sortByCol = queryParams.SortBy.ToString().ToUpper();
            queryBuilder.Append($"ORDER BY [{sortOnCol}] {sortByCol} OFFSET @Offset ROWS FETCH NEXT @Count ROWS ONLY");

            var rowNoStart = queryParams.Page * queryParams.Count;

            using IDbConnection connection = new SqlConnection(_options.ConnectionString);
            var logs = await connection.QueryAsync<SqlServerLogModel>(queryBuilder.ToString(),
                new
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
                    item.RowNo = rowNoStart + i;
                    return item;
                })
                .ToList();
        }

        private async Task<int> CountLogsAsync(FetchLogsQuery queryParams)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"SELECT COUNT(Id) FROM [{_options.Schema}].[{_options.TableName}]");

            GenerateWhereClause(queryBuilder, queryParams);

            using IDbConnection connection = new SqlConnection(_options.ConnectionString);
            return await connection.ExecuteScalarAsync<int>(queryBuilder.ToString(),
                new
                {
                    queryParams.Level,
                    Search = queryParams.SearchCriteria != null ? "%" + queryParams.SearchCriteria + "%" : null,
                    queryParams.StartDate,
                    queryParams.EndDate
                });
        }

        private static void GenerateWhereClause(StringBuilder queryBuilder, FetchLogsQuery queryParams)
        {
            var conditionStart = "WHERE";

            if (!string.IsNullOrEmpty(queryParams.Level))
            {
                queryBuilder.Append($"{conditionStart} [{ColumnLevelName}] = @Level ");
                conditionStart = "AND";
            }

            if (!string.IsNullOrEmpty(queryParams.SearchCriteria))
            {
                queryBuilder.Append($"{conditionStart} [{ColumnMessageName}] LIKE @Search OR [Exception] LIKE @Search ");
                conditionStart = "AND";
            }

            if (queryParams.StartDate != null)
            {
                queryBuilder.Append($"{conditionStart} [{ColumnTimestampName}] >= @StartDate ");
                conditionStart = "AND";
            }

            if (queryParams.EndDate != null)
            {
                queryBuilder.Append($"{conditionStart} [{ColumnTimestampName}] <= @EndDate ");
            }
        }

        private static string GetColumnName(SortProperty sortOn)
            => sortOn switch
            {
                SortProperty.Level => ColumnLevelName,
                SortProperty.Message => ColumnMessageName,
                SortProperty.Timestamp => ColumnTimestampName,
                _ => ColumnTimestampName
            };
    }
}