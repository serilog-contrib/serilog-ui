using Serilog.Ui.Core.Models;
using Serilog.Ui.Core.QueryBuilder.Sql;
using Serilog.Ui.PostgreSqlProvider.Models;
using System;
using System.Text;
using static Serilog.Ui.Core.Models.SearchOptions;

namespace Serilog.Ui.PostgreSqlProvider;

/// <summary>
/// Provides methods to build SQL queries specifically for PostgreSQL to fetch and count logs.
/// </summary>
/// <typeparam name="TModel">The type of the log model.</typeparam>
public class PostgresQueryBuilder<TModel> : SqlQueryBuilder<TModel> where TModel : LogModel
{
    ///<inheritdoc />
    public override string BuildFetchLogsQuery(SinkColumnNames columns, string schema, string tableName, FetchLogsQuery query)
    {
        StringBuilder queryStr = new();

        GenerateSelectClause(queryStr, columns, schema, tableName);

        GenerateWhereClause(queryStr, columns, query.Level, query.SearchCriteria, query.StartDate, query.EndDate);

        queryStr.Append($"{GenerateSortClause(columns, query.SortOn, query.SortBy)} LIMIT @Count OFFSET @Offset");

        return queryStr.ToString();
    }

    ///<inheritdoc />
    public override string BuildCountLogsQuery(SinkColumnNames columns, string schema, string tableName, FetchLogsQuery query)
    {
        StringBuilder queryStr = new();

        queryStr.Append($"SELECT COUNT(\"{columns.Message}\") ")
                .Append($"FROM \"{schema}\".\"{tableName}\"");

        GenerateWhereClause(queryStr, columns, query.Level, query.SearchCriteria, query.StartDate, query.EndDate);

        return queryStr.ToString();
    }

    ///<inheritdoc />
    protected override string GenerateSortClause(SinkColumnNames columns, SortProperty sortOn, SortDirection sortBy)
        => $"ORDER BY \"{GetSortColumnName(columns, sortOn)}\" {sortBy.ToString().ToUpper()}";

    /// <summary>
    /// Generates the SELECT clause for the SQL query.
    /// </summary>
    /// <param name="queryBuilder">The StringBuilder to append the SELECT clause to.</param>
    /// <param name="columns">The column names used in the sink for logging.</param>
    /// <param name="schema">The schema of the table.</param>
    /// <param name="tableName">The name of the table.</param>
    private static void GenerateSelectClause(StringBuilder queryBuilder, SinkColumnNames columns, string schema, string tableName)
    {
        if (typeof(TModel) != typeof(PostgresLogModel))
        {
            queryBuilder.Append("SELECT *");
        }
        else
        {
            queryBuilder.Append($"SELECT \"{columns.Message}\", ")
                .Append($"\"{columns.MessageTemplate}\", ")
                .Append($"\"{columns.Level}\", ")
                .Append($"\"{columns.Timestamp}\", ")
                .Append($"\"{columns.Exception}\", ")
                .Append($"\"{columns.LogEventSerialized}\" AS \"Properties\"");
        }

        queryBuilder.Append($" FROM \"{schema}\".\"{tableName}\" ");
    }

    /// <summary>
    /// Generates the WHERE clause for the SQL query.
    /// </summary>
    /// <param name="queryBuilder">The StringBuilder to append the WHERE clause to.</param>
    /// <param name="columns">The column names used in the sink for logging.</param>
    /// <param name="level">The log level to filter by.</param>
    /// <param name="searchCriteria">The search criteria to filter by.</param>
    /// <param name="startDate">The start date to filter by.</param>
    /// <param name="endDate">The end date to filter by.</param>
    private static void GenerateWhereClause(
        StringBuilder queryBuilder,
        SinkColumnNames columns,
        string? level,
        string? searchCriteria,
        DateTime? startDate,
        DateTime? endDate)
    {
        StringBuilder conditions = new();

        if (!string.IsNullOrWhiteSpace(level))
        {
            conditions.Append($"AND \"{columns.Level}\" = @Level ");
        }

        if (!string.IsNullOrWhiteSpace(searchCriteria))
        {
            conditions.Append($"AND (\"{columns.Message}\" LIKE @Search ");
            conditions.Append(AddExceptionToWhereClause() ? $"OR \"{columns.Exception}\" LIKE @Search) " : ") ");
        }

        if (startDate.HasValue)
        {
            conditions.Append($"AND \"{columns.Timestamp}\" >= @StartDate ");
        }

        if (endDate.HasValue)
        {
            conditions.Append($"AND \"{columns.Timestamp}\" <= @EndDate ");
        }

        if (conditions.Length <= 0)
        {
            return;
        }

        queryBuilder
            .Append("WHERE TRUE ")
            .Append(conditions);
    }
}