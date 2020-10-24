using Dapper;
using Serilog.Ui.Core;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Ui.MsSqlServerProvider
{
    public class SqlServerDataProvider : IDataProvider
    {
        private readonly RelationalDbOptions _options;

        public SqlServerDataProvider(RelationalDbOptions options)
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
            queryBuilder.Append("SELECT [Id], [Message], [Level], [TimeStamp], [Exception], [Properties] FROM[");
            queryBuilder.Append(_options.Schema);
            queryBuilder.Append("].[");
            queryBuilder.Append(_options.TableName);
            queryBuilder.Append("] ");

            var whereIncluded = false;

            if (!string.IsNullOrEmpty(level))
            {
                queryBuilder.Append("WHERE [LEVEL] = @Level ");
                whereIncluded = true;
            }

            if (!string.IsNullOrEmpty(searchCriteria))
            {
                queryBuilder.Append(whereIncluded
                    ? "AND [Message] LIKE @Search OR [Exception] LIKE @Search "
                    : "WHERE [Message] LIKE @Search OR [Exception] LIKE @Search ");
            }

            queryBuilder.Append("ORDER BY Id DESC OFFSET @Offset ROWS FETCH NEXT @Count ROWS ONLY");

            using (IDbConnection connection = new SqlConnection(_options.ConnectionString))
            {
                return await connection.QueryAsync<SqlServerLogModel>(queryBuilder.ToString(),
                    new
                    {
                        Offset = page,
                        Count = count,
                        Level = level,
                        Search = searchCriteria != null ? "%" + searchCriteria + "%" : null
                    });
            }
        }

        public async Task<int> CountLogsAsync(string level, string searchCriteria)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT COUNT(Id) FROM[");
            queryBuilder.Append(_options.Schema);
            queryBuilder.Append("].[");
            queryBuilder.Append(_options.TableName);
            queryBuilder.Append("] ");

            var whereIncluded = false;

            if (!string.IsNullOrEmpty(level))
            {
                queryBuilder.Append("WHERE [LEVEL] = @Level ");
                whereIncluded = true;
            }

            if (!string.IsNullOrEmpty(searchCriteria))
            {
                queryBuilder.Append(whereIncluded
                    ? "AND [Message] LIKE @Search OR [Exception] LIKE @Search "
                    : "WHERE [Message] LIKE @Search OR [Exception] LIKE @Search ");
            }

            using (IDbConnection connection = new SqlConnection(_options.ConnectionString))
            {
                return await connection.ExecuteScalarAsync<int>(queryBuilder.ToString(),
                    new
                    {
                        Level = level,
                        Search = searchCriteria != null ? "%" + searchCriteria + "%" : null
                    });
            }
        }
    }
}