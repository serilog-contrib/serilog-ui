using Dapper;
using MySql.Data.MySqlClient;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Serilog.Ui.Core.Models.SearchOptions;

namespace Serilog.Ui.MySqlProvider
{
    public class MySqlDataProvider(RelationalDbOptions options) : IDataProvider
    {
        private readonly RelationalDbOptions _options = options ?? throw new ArgumentNullException(nameof(options));

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
            var logsTask = GetLogsAsync(page - 1, count, level, searchCriteria, startDate, endDate, sortOn, sortBy);
            var logCountTask = CountLogsAsync(level, searchCriteria, startDate, endDate);

            await Task.WhenAll(logsTask, logCountTask);

            return (await logsTask, await logCountTask);
        }

        public string Name => _options.ToDataProviderName("MySQL");

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
            queryBuilder.Append($"SELECT Id, Message, LogLevel AS `Level`, TimeStamp, Exception, Properties From `{_options.TableName}` ");

            GenerateWhereClause(queryBuilder, level, searchCriteria, startDate, endDate);
            var sortClause = GenerateSortClause(sortOn, sortBy);

            queryBuilder.Append($"ORDER BY {sortClause} LIMIT @Offset, @Count");

            using var connection = new MySqlConnection(_options.ConnectionString);
            var param = new
            {
                Offset = page * count,
                Count = count,
                Level = level,
                Search = searchCriteria != null ? $"%{searchCriteria}%" : null,
                StartDate = startDate,
                EndDate = endDate
            };
            var logs = await connection.QueryAsync<MySqlLogModel>(queryBuilder.ToString(), param);

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
            queryBuilder.Append($"SELECT COUNT(Id) FROM `{_options.TableName}` ");

            GenerateWhereClause(queryBuilder, level, searchCriteria, startDate, endDate);

            using var connection = new MySqlConnection(_options.ConnectionString);
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
            var whereIncluded = false;

            if (!string.IsNullOrEmpty(level))
            {
                queryBuilder.Append("WHERE LogLevel = @Level ");
                whereIncluded = true;
            }

            if (!string.IsNullOrEmpty(searchCriteria))
            {
                queryBuilder.Append(whereIncluded
                    ? "AND (Message LIKE @Search OR Exception LIKE @Search) "
                    : "WHERE (Message LIKE @Search OR Exception LIKE @Search) ");
                whereIncluded = true;
            }

            if (startDate != null)
            {
                queryBuilder.Append(whereIncluded
                    ? "AND TimeStamp >= @StartDate "
                    : "WHERE TimeStamp >= @StartDate ");
                whereIncluded = true;
            }

            if (endDate != null)
            {
                queryBuilder.Append(whereIncluded
                    ? "AND TimeStamp <= @EndDate "
                    : "WHERE TimeStamp <= @EndDate ");
            }
        }

        private static string GenerateSortClause(SortProperty sortOn, SortDirection sortBy)
        {
            var sortProperty = sortOn == SortProperty.Level ? "LogLevel" : sortOn.ToString();
            return $"{sortProperty} {sortBy.ToString().ToUpper()}";
        }
    }
}