using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Dapper;
using Microsoft.Data.SqlClient;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Attributes;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.Core.Models;
using Serilog.Ui.Core.OptionsBuilder;
using static Serilog.Ui.Core.Models.SearchOptions;

namespace Serilog.Ui.MsSqlServerProvider
{
    public class SqlServerDataProvider(RelationalDbOptions options) : SqlServerDataProvider<SqlServerLogModel>(options)
    {
        protected override string SearchCriteriaWhereQuery() => "OR [Exception] LIKE @Search";

        protected override string SelectQuery()
        {
            const string level = $"[{ColumnLevelName}]";
            const string message = $"[{ColumnMessageName}]";
            const string timestamp = $"[{ColumnTimestampName}]";

            return $"SELECT [Id], {message}, {level}, {timestamp}, [Exception], [Properties] ";
        }
    }

    public class SqlServerDataProvider<T>(RelationalDbOptions options) : IDataProvider
        where T : SqlServerLogModel
    {
        internal const string MsSqlProviderName = "MsSQL";

        private protected const string ColumnTimestampName = "TimeStamp";

        private protected const string ColumnLevelName = "Level";

        private protected const string ColumnMessageName = "Message";

        private readonly RelationalDbOptions _options = Guard.Against.Null(options);

        public string Name => _options.ToDataProviderName(MsSqlProviderName);

        protected virtual string SelectQuery() => "SELECT * ";

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

            queryBuilder.Append(SelectQuery());
            queryBuilder.Append($"FROM [{_options.Schema}].[{_options.TableName}] ");

            GenerateWhereClause(queryBuilder, queryParams);

            GenerateSortClause(queryBuilder, queryParams.SortOn, queryParams.SortBy);

            queryBuilder.Append("OFFSET @Offset ROWS FETCH NEXT @Count ROWS ONLY");

            var rowNoStart = queryParams.Page * queryParams.Count;

            using IDbConnection connection = new SqlConnection(_options.ConnectionString);
            var logs = await connection.QueryAsync<T>(queryBuilder.ToString(),
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

        /// <summary>
        /// If Exception property is flagged with <see cref="RemovedColumnAttribute"/>,
        /// it removes the Where query part on the Exception field. 
        /// </summary>
        /// <returns></returns>
        protected virtual string SearchCriteriaWhereQuery()
        {
            var exceptionProperty = typeof(T).GetProperty(nameof(SqlServerLogModel.Exception));
            var att = exceptionProperty?.GetCustomAttribute<RemovedColumnAttribute>();
            return att is null ? "OR [Exception] LIKE @Search" : string.Empty;
        }

        private void GenerateWhereClause(StringBuilder queryBuilder, FetchLogsQuery queryParams)
        {
            var conditionStart = "WHERE";

            if (!string.IsNullOrEmpty(queryParams.Level))
            {
                queryBuilder.Append($"{conditionStart} [{ColumnLevelName}] = @Level ");
                conditionStart = "AND";
            }

            if (!string.IsNullOrEmpty(queryParams.SearchCriteria))
            {
                queryBuilder.Append($"{conditionStart} [{ColumnMessageName}] LIKE @Search {SearchCriteriaWhereQuery()} ");
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

        private static void GenerateSortClause(StringBuilder queryBuilder, SortProperty sortOn, SortDirection sortBy)
        {
            var sortOnCol = GetColumnName(sortOn);
            var sortByCol = sortBy.ToString().ToUpper();

            queryBuilder.Append($"ORDER BY [{sortOnCol}] {sortByCol} ");
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