using Dapper;
using Microsoft.Data.Sqlite;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Ui.SqliteDataProvider
{
    public class SqliteDataProvider : IDataProvider
    {
        private readonly RelationalDbOptions _options;

        public SqliteDataProvider(RelationalDbOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(
            int page,
            int count,
            string level = null,
            string searchCriteria = null,
            DateTime? startDate = null,
            DateTime? endDate = null
        )
        {
            var logsTask = GetLogs(page - 1, count, level, searchCriteria, startDate, endDate);
            var logCountTask = CountLogs(level, searchCriteria, startDate, endDate);

            await Task.WhenAll(logsTask, logCountTask);

            return (await logsTask, await logCountTask);
        }

        public string Name => _options.ToDataProviderName("Sqlite");

        private Task<IEnumerable<LogModel>> GetLogs(
            int page,
            int count,
            string level,
            string searchCriteria,
            DateTime? startDate,
            DateTime? endDate)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT Id, RenderedMessage AS Message, Level, Timestamp, Exception, Properties FROM ");
            queryBuilder.Append(_options.TableName);
            queryBuilder.Append(" ");

            GenerateWhereClause(queryBuilder, level, searchCriteria, startDate, endDate);

            queryBuilder.Append("ORDER BY Id DESC LIMIT @Offset, @Count");

            using (var connection = new SqliteConnection(_options.ConnectionString))
            {
                var param = new
                {
                    Offset = page * count,
                    Count = count,
                    Level = level,
                    Search = searchCriteria != null ? $"%{searchCriteria}%" : null,
                    StartDate = startDate,
                    EndDate = endDate
                };
                var logs = connection.Query<LogModel>(queryBuilder.ToString(), param);
                var index = 1;
                foreach (var log in logs)
                    log.RowNo = (page * count) + index++;

                return Task.FromResult(logs);
            }
        }

        private Task<int> CountLogs(
            string level,
            string searchCriteria,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT COUNT(Id) FROM ");
            queryBuilder.Append(_options.TableName);
            queryBuilder.Append(" ");

            GenerateWhereClause(queryBuilder, level, searchCriteria, startDate, endDate);

            using (var connection = new SqliteConnection(_options.ConnectionString))
            {
                return Task.FromResult(connection.QueryFirstOrDefault<int>(queryBuilder.ToString(),
                    new
                    {
                        Level = level,
                        Search = searchCriteria != null ? "%" + searchCriteria + "%" : null,
                        StartDate = startDate,
                        EndDate = endDate
                    }));
            }
        }

        private void GenerateWhereClause(
            StringBuilder queryBuilder,
            string level,
            string searchCriteria,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var whereIncluded = false;

            if (!string.IsNullOrEmpty(level))
            {
                queryBuilder.Append("WHERE Level = @Level ");
                whereIncluded = true;
            }

            if (!string.IsNullOrEmpty(searchCriteria))
            {
                queryBuilder.Append(whereIncluded
                    ? "AND (RenderedMessage LIKE @Search OR Exception LIKE @Search) "
                    : "WHERE (RenderedMessage LIKE @Search OR Exception LIKE @Search) ");
                whereIncluded = true;
            }

            if (startDate != null)
            {
                queryBuilder.Append(whereIncluded
                    ? "AND Timestamp >= @StartDate "
                    : "WHERE Timestamp >= @StartDate ");
                whereIncluded = true;
            }

            if (endDate != null)
            {
                queryBuilder.Append(whereIncluded
                    ? "AND Timestamp <= @EndDate "
                    : "WHERE Timestamp <= @EndDate ");
            }
        }
    }
}
