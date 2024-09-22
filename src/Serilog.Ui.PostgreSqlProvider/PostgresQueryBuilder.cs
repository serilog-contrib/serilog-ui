using Serilog.Ui.Core.Attributes;
using Serilog.Ui.Core.Models;
using Serilog.Ui.Core.QueryBuilder.Sql;
using Serilog.Ui.PostgreSqlProvider.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Serilog.Ui.PostgreSqlProvider;

/// <summary>
/// Provides methods to build SQL queries specifically for PostgreSQL to fetch and count logs.
/// </summary>
public class PostgresQueryBuilder : SqlQueryBuilder
{
    /// <summary>
    /// Builds a SQL query to fetch logs from the specified PostgreSQL table.
    /// </summary>
    /// <typeparam name="T">The type of the log model.</typeparam>
    /// <param name="columns">The column names used in the sink for logging.</param>
    /// <param name="schema">The schema of the table.</param>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="query">The query parameters for fetching logs.</param>
    /// <returns>A SQL query string to fetch logs.</returns>
    public override string BuildFetchLogsQuery<T>(SinkColumnNames columns, string schema, string tableName, FetchLogsQuery query)
    {
        StringBuilder queryStr = new();

        GenerateSelectClause<T>(queryStr, columns, schema, tableName);

        GenerateWhereClause<T>(queryStr, columns, query.Level, query.SearchCriteria, query.StartDate, query.EndDate);

        string sortClause = GenerateSortClause(columns, query.SortOn, query.SortBy);

        queryStr.Append($" ORDER BY {sortClause} LIMIT @Count OFFSET @Offset");

        return queryStr.ToString();
    }

    /// <summary>
    /// Builds a SQL query to count logs in the specified PostgreSQL table.
    /// </summary>
    /// <typeparam name="T">The type of the log model.</typeparam>
    /// <param name="columns">The column names used in the sink for logging.</param>
    /// <param name="schema">The schema of the table.</param>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="query">The query parameters for counting logs.</param>
    /// <returns>A SQL query string to count logs.</returns>
    public override string BuildCountLogsQuery<T>(SinkColumnNames columns, string schema, string tableName, FetchLogsQuery query)
    {
        StringBuilder queryStr = new();

        queryStr.Append($"SELECT COUNT(\"{columns.Message}\") ")
                .Append($"FROM \"{schema}\".\"{tableName}\"");

        GenerateWhereClause<T>(queryStr, columns, query.Level, query.SearchCriteria, query.StartDate, query.EndDate);

        return queryStr.ToString();
    }

    /// <summary>
    /// Generates the SELECT clause for the SQL query.
    /// </summary>
    /// <typeparam name="T">The type of the log model.</typeparam>
    /// <param name="queryBuilder">The StringBuilder to append the SELECT clause to.</param>
    /// <param name="columns">The column names used in the sink for logging.</param>
    /// <param name="schema">The schema of the table.</param>
    /// <param name="tableName">The name of the table.</param>
    private static void GenerateSelectClause<T>(StringBuilder queryBuilder, SinkColumnNames columns, string schema, string tableName)
        where T : LogModel
    {
        if (typeof(T) != typeof(PostgresLogModel))
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

        queryBuilder.Append($" FROM \"{schema}\".\"{tableName}\"");
    }

    /// <summary>
    /// Generates the WHERE clause for the SQL query.
    /// </summary>
    /// <typeparam name="T">The type of the log model.</typeparam>
    /// <param name="queryBuilder">The StringBuilder to append the WHERE clause to.</param>
    /// <param name="columns">The column names used in the sink for logging.</param>
    /// <param name="level">The log level to filter by.</param>
    /// <param name="searchCriteria">The search criteria to filter by.</param>
    /// <param name="startDate">The start date to filter by.</param>
    /// <param name="endDate">The end date to filter by.</param>
    private static void GenerateWhereClause<T>(
        StringBuilder queryBuilder,
        SinkColumnNames columns,
        string? level,
        string? searchCriteria,
        DateTime? startDate,
        DateTime? endDate)
        where T : LogModel
    {
        List<string> conditions = new();

        if (!string.IsNullOrWhiteSpace(level))
        {
            conditions.Add($"\"{columns.Level}\" = @Level");
        }

        if (!string.IsNullOrWhiteSpace(searchCriteria))
        {
            string exceptionCondition = AddExceptionToWhereClause<T>() ? $"OR \"{columns.Exception}\" LIKE @Search" : string.Empty;
            conditions.Add($"(\"{columns.Message}\" LIKE @Search {exceptionCondition})");
        }

        if (startDate.HasValue)
        {
            conditions.Add($"\"{columns.Timestamp}\" >= @StartDate");
        }

        if (endDate.HasValue)
        {
            conditions.Add($"\"{columns.Timestamp}\" <= @EndDate");
        }

        if (conditions.Count <= 0)
        {
            return;
        }

        queryBuilder
            .Append(" WHERE TRUE AND ")
            .Append(string.Join(" AND ", conditions));
    }

    /// <summary>
    /// Determines whether to add the exception column to the WHERE clause based on the log model type.
    /// </summary>
    /// <typeparam name="T">The type of the log model.</typeparam>
    /// <returns>True if the exception column should be added; otherwise, false.</returns>
    private static bool AddExceptionToWhereClause<T>()
        where T : LogModel
    {
        PropertyInfo? exceptionProperty = typeof(T).GetProperty(nameof(PostgresLogModel.Exception));
        RemovedColumnAttribute? att = exceptionProperty?.GetCustomAttribute<RemovedColumnAttribute>();

        return att is null;
    }
}