using Dapper;
using Npgsql;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Ui.PostgreSqlProvider
{
    public class PostgresDataProvider : IDataProvider
    {
        private readonly RelationalDbOptions _options;

        public PostgresDataProvider(RelationalDbOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(
            int page,
            int count,
            string logLevel = null,
            string searchCriteria = null,
            DateTime? startDate = null,
            DateTime? endDate = null
        )
        {
            var logsTask = GetLogsAsync(page - 1, count, logLevel, searchCriteria, startDate, endDate);
            var logCountTask = CountLogsAsync(logLevel, searchCriteria);

            await Task.WhenAll(logsTask, logCountTask);

            return (await logsTask, await logCountTask);
        }

        private async Task<IEnumerable<LogModel>> GetLogsAsync(int page,
            int count,
            string level,
            string searchCriteria,
            DateTime? startDate,
            DateTime? endDate)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT message, message_template, level, timestamp, exception, log_event AS \"Properties\" FROM ");
            queryBuilder.Append(_options.Schema);
            queryBuilder.Append(".");
            queryBuilder.Append(_options.TableName);

            GenerateWhereClause(queryBuilder, level, searchCriteria, startDate, endDate);

            queryBuilder.Append(" ORDER BY timestamp DESC LIMIT @Count OFFSET @Offset ");

            using IDbConnection connection = new NpgsqlConnection(_options.ConnectionString);
            var logs = await connection.QueryAsync<PostgresLogModel>(queryBuilder.ToString(),
                new
                {
                    Offset = page * count,
                    Count = count,
                    Level = LogLevelConverter.GetLevelValue(level),
                    Search = searchCriteria != null ? "%" + searchCriteria + "%" : null,
                    StartDate = startDate,
                    EndDate = endDate
                });

            var index = 1;
            foreach (var log in logs)
                log.RowNo = (page * count) + index++;

            return logs;
        }

        private async Task<int> CountLogsAsync(
            string level,
            string searchCriteria,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT COUNT(message) FROM ");
            queryBuilder.Append(_options.Schema);
            queryBuilder.Append(".");
            queryBuilder.Append(_options.TableName);

            GenerateWhereClause(queryBuilder, level, searchCriteria, startDate, endDate);

            using IDbConnection connection = new NpgsqlConnection(_options.ConnectionString);
            return await connection.ExecuteScalarAsync<int>(queryBuilder.ToString(),
                new
                {
                    Level = LogLevelConverter.GetLevelValue(level),
                    Search = searchCriteria != null ? "%" + searchCriteria + "%" : null,
                    StartDate = startDate,
                    EndDate = endDate
                });
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
                queryBuilder.Append(" WHERE level = @Level ");
                whereIncluded = true;
            }

            if (!string.IsNullOrEmpty(searchCriteria))
            {
                queryBuilder.Append(whereIncluded
                    ? " AND message LIKE @Search OR exception LIKE @Search "
                    : " WHERE message LIKE @Search OR exception LIKE @Search ");
            }

            if (startDate != null)
            {
                queryBuilder.Append(whereIncluded
                    ? " AND timestamp >= @StartDate "
                    : " WHERE timestamp >= @StartDate ");
                whereIncluded = true;
            }

            if (endDate != null)
            {
                queryBuilder.Append(whereIncluded
                    ? " AND timestamp < @EndDate "
                    : " WHERE timestamp < @EndDate ");
            }
        }
    }
}