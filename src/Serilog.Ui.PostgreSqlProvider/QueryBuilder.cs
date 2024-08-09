using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Serilog.Ui.Core.Attributes;
using Serilog.Ui.Core.Models;
using Serilog.Ui.PostgreSqlProvider.Models;
using static Serilog.Ui.Core.Models.SearchOptions;

namespace Serilog.Ui.PostgreSqlProvider;

internal static class QueryBuilder
{
    internal static string BuildFetchLogsQuery<T>(this SinkColumnNames _columns, string schema, string tableName, FetchLogsQuery query)
        where T : PostgresLogModel
    {
        var sortClause = _columns.GenerateSortClause(query.SortOn, query.SortBy);

        return new StringBuilder()
            .GenerateSelectClause<T>(_columns)
            .Append($" FROM \"{schema}\".\"{tableName}\"")
            .GenerateWhereClause<T>(_columns, query.Level, query.SearchCriteria, query.StartDate, query.EndDate)
            .Append($" ORDER BY {sortClause} LIMIT @Count OFFSET @Offset")
            .ToString();
    }

    internal static string BuildCountLogsQuery<T>(this SinkColumnNames _columns, string schema, string tableName, FetchLogsQuery query)
        where T : PostgresLogModel
    {
        return new StringBuilder()
            .Append($"SELECT COUNT(\"{_columns.RenderedMessage}\") ")
            .Append($"FROM \"{schema}\".\"{tableName}\"")
            .GenerateWhereClause<T>(_columns, query.Level, query.SearchCriteria, query.StartDate, query.EndDate)
            .ToString();
    }

    private static StringBuilder GenerateSelectClause<T>(this StringBuilder queryBuilder, SinkColumnNames _columns)
        where T : PostgresLogModel
    {
        if (typeof(T) != typeof(PostgresLogModel))
        {
            return queryBuilder.Append("SELECT *");
        }

        return queryBuilder.Append($"SELECT \"{_columns.RenderedMessage}\", ")
            .Append($"\"{_columns.MessageTemplate}\", ")
            .Append($"\"{_columns.Level}\", ")
            .Append($"\"{_columns.Timestamp}\", ")
            .Append($"\"{_columns.Exception}\", ")
            .Append($"\"{_columns.LogEventSerialized}\" AS \"Properties\"");
    }

    private static StringBuilder GenerateWhereClause<T>(this StringBuilder queryBuilder,
        SinkColumnNames _columns,
        string? level,
        string? searchCriteria,
        DateTime? startDate,
        DateTime? endDate)
        where T : PostgresLogModel
    {
        var conditions = new List<string>();

        if (!string.IsNullOrWhiteSpace(level))
        {
            conditions.Add($"\"{_columns.Level}\" = @Level");
        }

        if (!string.IsNullOrWhiteSpace(searchCriteria))
        {
            var exceptionCondition = AddExceptionToWhereClause<T>() ? $"OR \"{_columns.Exception}\" LIKE @Search" : string.Empty;
            conditions.Add($"(\"{_columns.RenderedMessage}\" LIKE @Search {exceptionCondition})");
        }

        if (startDate.HasValue)
        {
            conditions.Add($"\"{_columns.Timestamp}\" >= @StartDate");
        }

        if (endDate.HasValue)
        {
            conditions.Add($"\"{_columns.Timestamp}\" <= @EndDate");
        }

        if (conditions.Count <= 0) return queryBuilder;

        return queryBuilder
            .Append(" WHERE TRUE AND ")
            .Append(string.Join(" AND ", conditions));
    }

    private static bool AddExceptionToWhereClause<T>()
        where T : PostgresLogModel
    {
        var exceptionProperty = typeof(T).GetProperty(nameof(PostgresLogModel.Exception));
        var att = exceptionProperty?.GetCustomAttribute<RemovedColumnAttribute>();
        return att is null;
    }

    private static string GenerateSortClause(this SinkColumnNames _columns, SortProperty sortOn, SortDirection sortBy)
    {
        var sortPropertyName = sortOn switch
        {
            SortProperty.Timestamp => _columns.Timestamp,
            SortProperty.Level => _columns.Level,
            SortProperty.Message => _columns.RenderedMessage,
            _ => _columns.Timestamp,
        };

        return $"\"{sortPropertyName}\" {sortBy.ToString().ToUpper()}";
    }
}