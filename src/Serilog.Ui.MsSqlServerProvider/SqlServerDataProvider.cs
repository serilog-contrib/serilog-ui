using Dapper;
using Microsoft.Data.SqlClient;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            // since sink stores dates in local time, we query by local time
            var localStartDate = startDate?.ToLocalTime();
            var localEndDate = endDate?.ToLocalTime();

            var logsTask = GetLogsAsync(page - 1, count, level, searchCriteria, localStartDate, localEndDate, sortOn, sortBy);
            var logCountTask = CountLogsAsync(level, searchCriteria, localStartDate, localEndDate);

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
            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"SELECT [Id], [{ColumnMessageName}], [{ColumnLevelName}], [{ColumnTimestampName}], [Exception], [Properties] ");
            queryBuilder.Append($"FROM [{_options.Schema}].[{_options.TableName}] ");

            GenerateWhereClause(queryBuilder, level, searchCriteria, startDate, endDate);

            var sortOnCol = GetColumnName(sortOn);
            var sortByCol = sortBy.ToString().ToUpper();
            queryBuilder.Append($"ORDER BY [{sortOnCol}] {sortByCol} OFFSET @Offset ROWS FETCH NEXT @Count ROWS ONLY");

            using IDbConnection connection = new SqlConnection(_options.ConnectionString);
            var logs = await connection.QueryAsync<SqlServerLogModel>(queryBuilder.ToString(),
                new
                {
                    Offset = page * count,
                    Count = count,
                    Level = level,
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
            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"SELECT COUNT(Id) FROM [{_options.Schema}].[{_options.TableName}]");

            GenerateWhereClause(queryBuilder, level, searchCriteria, startDate, endDate);

            using IDbConnection connection = new SqlConnection(_options.ConnectionString);
            return await connection.ExecuteScalarAsync<int>(queryBuilder.ToString(),
                new
                {
                    Level = level,
                    Search = searchCriteria != null ? "%" + searchCriteria + "%" : null,
                    StartDate = startDate,
                    EndDate = endDate
                });
        }

        private static void GenerateWhereClause(
            StringBuilder queryBuilder,
            string level,
            string searchCriteria,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var conditionStart = "WHERE";

            if (!string.IsNullOrEmpty(level))
            {
                queryBuilder.Append($"{conditionStart} [{ColumnLevelName}] = @Level ");
                conditionStart = "AND";
            }

            if (!string.IsNullOrEmpty(searchCriteria))
            {
                queryBuilder.Append($"{conditionStart} [{ColumnMessageName}] LIKE @Search OR [Exception] LIKE @Search ");
                conditionStart = "AND";
            }

            if (startDate != null)
            {
                queryBuilder.Append($"{conditionStart} [{ColumnTimestampName}] >= @StartDate ");
                conditionStart = "AND";
            }

            if (endDate != null)
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