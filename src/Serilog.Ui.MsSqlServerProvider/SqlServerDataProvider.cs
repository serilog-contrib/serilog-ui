using Dapper;
using Serilog.Ui.Core;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(int page, int count)
        {
            using (IDbConnection connection = new SqlConnection(_options.ConnectionString))
            {
                var logsTask = GetLogsAsync(connection, page - 1, count);
                var logCountTask = CountLogsAsync(connection);

                await Task.WhenAll(logsTask, logCountTask);

                return (await logsTask, await logCountTask);
            }
        }

        private Task<IEnumerable<LogModel>> GetLogsAsync(IDbConnection connection, int page, int count)
        {
            var query =
                $"SELECT [Id], [Message],[Level], [TimeStamp], [Exception], [Properties] FROM [{_options.Schema}].[{_options.TableName}] " +
                "ORDER BY Id DESC OFFSET @Offset ROWS FETCH NEXT @Count ROWS ONLY";

            return connection.QueryAsync<LogModel>(query, new { page, count });
        }

        public async Task<int> CountLogsAsync(IDbConnection connection)
        {
            var query = $"SELECT COUNT(Id) FROM [{_options.Schema}].[{_options.TableName}]";

            return await connection.ExecuteScalarAsync<int>(query);
        }
    }
}