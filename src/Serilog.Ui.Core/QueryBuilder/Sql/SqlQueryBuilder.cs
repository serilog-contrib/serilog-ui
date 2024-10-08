﻿using Serilog.Ui.Core.Attributes;
using Serilog.Ui.Core.Models;
using System.Reflection;
using static Serilog.Ui.Core.Models.SearchOptions;

namespace Serilog.Ui.Core.QueryBuilder.Sql;

/// <summary>
/// Abstract class that provides methods to build SQL queries for fetching and counting logs.
/// </summary>
public abstract class SqlQueryBuilder<TModel> where TModel : LogModel
{
    /// <summary>
    /// Builds a SQL query to fetch logs from the specified table.
    /// </summary>
    /// <param name="columns">The column names used in the sink for logging.</param>
    /// <param name="schema">The schema of the table.</param>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="query">The query parameters for fetching logs.</param>
    /// <returns>A SQL query string to fetch logs.</returns>
    public abstract string BuildFetchLogsQuery(SinkColumnNames columns, string schema, string tableName, FetchLogsQuery query);

    /// <summary>
    /// Builds a SQL query to count logs in the specified table.
    /// </summary>
    /// <param name="columns">The column names used in the sink for logging.</param>
    /// <param name="schema">The schema of the table.</param>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="query">The query parameters for counting logs.</param>
    /// <returns>A SQL query string to count logs.</returns>
    public abstract string BuildCountLogsQuery(SinkColumnNames columns, string schema, string tableName, FetchLogsQuery query);

    /// <summary>
    /// Generates a SQL sort clause based on the specified sort property and direction.
    /// </summary>
    /// <param name="columns">The column names used in the sink for logging.</param>
    /// <param name="sortOn">The property to sort on.</param>
    /// <param name="sortBy">The direction to sort by.</param>
    /// <returns>A SQL sort clause string.</returns>
    protected abstract string GenerateSortClause(SinkColumnNames columns, SortProperty sortOn, SortDirection sortBy);

    /// <summary>
    /// Generates a SQL sort clause based on the specified sort property and direction.
    /// </summary>
    /// <param name="columns">The column names used in the sink for logging.</param>
    /// <param name="sortOn">The property to sort on.</param>
    /// <returns>A SQL sort clause string.</returns>
    protected static string GetSortColumnName(SinkColumnNames columns, SortProperty sortOn) => sortOn switch
    {
        SortProperty.Timestamp => columns.Timestamp,
        SortProperty.Level => columns.Level,
        SortProperty.Message => columns.Message,
        _ => columns.Timestamp
    };

    /// <summary>
    /// Determines whether to add the exception column to the WHERE clause based on the presence of the RemovedColumnAttribute.
    /// </summary>
    /// <returns>True if the exception column should be added to the WHERE clause; otherwise, false.</returns>
    protected static bool AddExceptionToWhereClause()
    {
        PropertyInfo? exceptionProperty = typeof(TModel).GetProperty("Exception");
        RemovedColumnAttribute? att = exceptionProperty?.GetCustomAttribute<RemovedColumnAttribute>();

        return att is null;
    }
}