using Dapper;
using Npgsql;
using Serilog.Ui.Core;
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
            _options = options;
        }

        public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(
            int page,
            int count,
            string logLevel = null,
            string searchCriteria = null
        )
        {
            var logsTask = GetLogsAsync(page - 1, count, logLevel, searchCriteria);
            var logCountTask = CountLogsAsync(logLevel, searchCriteria);

            await Task.WhenAll(logsTask, logCountTask);

            return (await logsTask, await logCountTask);
        }

        private async Task<IEnumerable<LogModel>> GetLogsAsync(int page, int count, string level, string searchCriteria)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT message, message_template, level, timestamp, exception, log_event FROM ");
            queryBuilder.Append(_options.Schema);
            queryBuilder.Append(".");
            queryBuilder.Append(_options.TableName);

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

            queryBuilder.Append(" ORDER BY timestamp DESC LIMIT @Count OFFSET @Offset ");

            using IDbConnection connection = new NpgsqlConnection(_options.ConnectionString);
            return await connection.QueryAsync<PostgresLogModel>(queryBuilder.ToString(),
                new
                {
                    Offset = page,
                    Count = count,
                    Level = LogLevelConverter.GetLevelValue(level),
                    Search = searchCriteria != null ? "%" + searchCriteria + "%" : null
                });
        }

        public async Task<int> CountLogsAsync(string level, string searchCriteria)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT COUNT(message) FROM ");
            queryBuilder.Append(_options.Schema);
            queryBuilder.Append(".");
            queryBuilder.Append(_options.TableName);

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

            using IDbConnection connection = new NpgsqlConnection(_options.ConnectionString);
            return await connection.ExecuteScalarAsync<int>(queryBuilder.ToString(),
                new
                {
                    Level = LogLevelConverter.GetLevelValue(level),
                    Search = searchCriteria != null ? "%" + searchCriteria + "%" : null
                });
        }
    }
}