using Serilog.Ui.PostgreSqlProvider.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Serilog.Ui.Core.Models;
using static Serilog.Ui.Core.Models.SearchOptions;

namespace Serilog.Ui.PostgreSqlProvider;

internal static class QueryBuilder
{
    internal static string BuildFetchLogsQuery(this SinkColumnNames _columns, string schema, string tableName, FetchLogsQuery query)
    {
        StringBuilder queryBuilder = new();

        queryBuilder
            .Append("SELECT ")
            .Append(
                $"\"{_columns.RenderedMessage}\", \"{_columns.MessageTemplate}\", \"{_columns.Level}\", \"{_columns.Timestamp}\", \"{_columns.Exception}\", \"{_columns.LogEventSerialized}\" AS \"Properties\"")
            .Append($" FROM \"{schema}\".\"{tableName}\"");

        _columns.GenerateWhereClause(queryBuilder, query.Level, query.SearchCriteria, query.StartDate, query.EndDate);
        var sortClause = _columns.GenerateSortClause(query.SortOn, query.SortBy);

        queryBuilder.Append($" ORDER BY {sortClause} LIMIT @Count OFFSET @Offset");

        return queryBuilder.ToString();
    }

    internal static string BuildCountLogsQuery(this SinkColumnNames _columns, string schema, string tableName, FetchLogsQuery query)
    {
        StringBuilder queryBuilder = new();

        queryBuilder
            .Append($"SELECT COUNT(\"{_columns.RenderedMessage}\")")
            .Append($" FROM \"{schema}\".\"{tableName}\"");

        _columns.GenerateWhereClause(queryBuilder, query.Level, query.SearchCriteria, query.StartDate, query.EndDate);

        return queryBuilder.ToString();
    }

    private static void GenerateWhereClause(this SinkColumnNames _columns,
        StringBuilder queryBuilder,
        string level,
        string searchCriteria,
        DateTime? startDate,
        DateTime? endDate)
    {
        var conditions = new List<string>();

        if (!string.IsNullOrEmpty(level))
        {
            conditions.Add($"\"{_columns.Level}\" = @Level");
        }

        if (!string.IsNullOrEmpty(searchCriteria))
        {
            conditions.Add($"(\"{_columns.RenderedMessage}\" LIKE @Search OR \"{_columns.Exception}\" LIKE @Search)");
        }

        if (startDate.HasValue)
        {
            conditions.Add($"\"{_columns.Timestamp}\" >= @StartDate");
        }

        if (endDate.HasValue)
        {
            conditions.Add($"\"{_columns.Timestamp}\" <= @EndDate");
        }

        if (conditions.Count <= 0) return;

        queryBuilder
            .Append(" WHERE TRUE AND ")
            .Append(string.Join(" AND ", conditions));
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