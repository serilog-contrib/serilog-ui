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

        public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(int page, int count, string level = null)
        {
            using (IDbConnection connection = new SqlConnection(_options.ConnectionString))
            {
                var logsTask = GetLogsAsync(page - 1, count, level);
                var logCountTask = CountLogsAsync();

                await Task.WhenAll(logsTask, logCountTask);

                return (await logsTask, await logCountTask);
            }
        }

        private async Task<IEnumerable<LogModel>> GetLogsAsync(int page, int count, string level)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT[Id], [Message],[Level], [TimeStamp], [Exception], [Properties] FROM[");
            queryBuilder.Append(_options.Schema);
            queryBuilder.Append("].[");
            queryBuilder.Append(_options.TableName);
            queryBuilder.Append("] ");

            if (!string.IsNullOrEmpty(level))
                queryBuilder.Append("WHERE [LEVEL] = @Level ");

            queryBuilder.Append("ORDER BY Id DESC OFFSET @Offset ROWS FETCH NEXT @Count ROWS ONLY");

            using (IDbConnection connection = new SqlConnection(_options.ConnectionString))
            {
                return await connection.QueryAsync<LogModel>(queryBuilder.ToString(), new { Offset = page, Count = count, Level = level });
            }
        }

        public async Task<int> CountLogsAsync()
        {
            var query = $"SELECT COUNT(Id) FROM [{_options.Schema}].[{_options.TableName}]";
            using (IDbConnection connection = new SqlConnection(_options.ConnectionString))
            {
                return await connection.ExecuteScalarAsync<int>(query);
            }
        }
    }
}