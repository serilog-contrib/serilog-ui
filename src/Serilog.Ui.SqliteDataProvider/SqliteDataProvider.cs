using Ardalis.GuardClauses;
using Dapper;
using Microsoft.Data.Sqlite;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;
using Serilog.Ui.Core.Models.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Serilog.Ui.Core.Models.SearchOptions;

namespace Serilog.Ui.SqliteDataProvider
{
    public class SqliteDataProvider(RelationalDbOptions options) : IDataProvider
    {
        internal const string SqliteProviderName = "SQLite";
        private readonly RelationalDbOptions _options = Guard.Against.Null(options);

        public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams, CancellationToken cancellationToken = default)
        {
            var logsTask = GetLogsAsync(queryParams);
            var logCountTask = CountLogsAsync(queryParams);

            await Task.WhenAll(logsTask, logCountTask);

            return (await logsTask, await logCountTask);
        }

        public string Name => _options.GetProviderName(SqliteProviderName);

        private async Task<IEnumerable<LogModel>> GetLogsAsync(FetchLogsQuery queryParams)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT Id, RenderedMessage AS Message, Level, Timestamp, Exception, Properties ");
            queryBuilder.Append($"FROM {_options.TableName} ");

            GenerateWhereClause(queryBuilder, queryParams);

            GenerateSortClause(queryBuilder, queryParams.SortOn, queryParams.SortBy);

            queryBuilder.Append("LIMIT @Offset, @Count");

            var rowNoStart = queryParams.Page * queryParams.Count;

            using var connection = new SqliteConnection(_options.ConnectionString);
            var queryParameters = new
            {
                Offset = rowNoStart,
                queryParams.Count,
                queryParams.Level,
                Search = queryParams.SearchCriteria != null ? $"%{queryParams.SearchCriteria}%" : null,
                queryParams.StartDate,
                queryParams.EndDate
            };
            var logs = await connection.QueryAsync<LogModel>(queryBuilder.ToString(), queryParameters);

            return logs.Select((item, i) => item.SetRowNo(rowNoStart, i)).ToList();
        }

        private Task<int> CountLogsAsync(FetchLogsQuery queryParams)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"SELECT COUNT(Id) FROM {_options.TableName} ");

            GenerateWhereClause(queryBuilder, queryParams);

            using var connection = new SqliteConnection(_options.ConnectionString);

            return connection.QueryFirstOrDefaultAsync<int>(
                queryBuilder.ToString(),
                new
                {
                    queryParams.Level,
                    Search = queryParams.SearchCriteria != null ? $"%{queryParams.SearchCriteria}%" : null,
                    queryParams.StartDate,
                    queryParams.EndDate
                });
        }

        private void GenerateWhereClause(StringBuilder queryBuilder, FetchLogsQuery queryParams)
        {
            var conditionStart = "WHERE";

            if (!string.IsNullOrWhiteSpace(queryParams.Level))
            {
                queryBuilder.Append($"{conditionStart} Level = @Level ");
                conditionStart = "AND";
            }

            if (!string.IsNullOrWhiteSpace(queryParams.SearchCriteria))
            {
                // TODO Exception as RemovableColumn?
                queryBuilder.Append($"{conditionStart} (RenderedMessage LIKE @Search OR Exception LIKE @Search) ");
                conditionStart = "AND";
            }

            if (queryParams.StartDate != null)
            {
                queryBuilder.Append($"{conditionStart} Timestamp >= @StartDate ");
                conditionStart = "AND";
            }

            if (queryParams.EndDate != null)
            {
                queryBuilder.Append($"{conditionStart} Timestamp <= @EndDate ");
            }
        }

        private void GenerateSortClause(StringBuilder queryBuilder, SortProperty sortOn, SortDirection sortBy)
        {
            // TODO
            queryBuilder.Append("ORDER BY Id DESC ");
        }
    }
}
