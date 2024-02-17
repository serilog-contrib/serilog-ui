using Serilog.Ui.PostgreSqlProvider.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static Serilog.Ui.Core.Models.SearchOptions;

namespace Serilog.Ui.PostgreSqlProvider;

internal static class QueryBuilder
{
    private static SinkColumnNames _columns;

    internal static void SetSinkType(PostgreSqlSinkType sinkType)
    {
        _columns = sinkType == PostgreSqlSinkType.SerilogSinksPostgreSQL
            ? new PostgreSqlSinkColumnNames()
            : new PostgreSqlAlternativeSinkColumnNames();
    }

    internal static string BuildFetchLogsQuery(
        string schema,
        string tableName,
        string level,
        string searchCriteria,
        ref DateTime? startDate,
        ref DateTime? endDate,
        SortProperty sortOn = SortProperty.Timestamp,
        SortDirection sortBy = SortDirection.Desc)
    {
        StringBuilder queryBuilder = new();

        queryBuilder
            .Append("SELECT ")
            .Append($"\"{_columns.RenderedMessage}\", \"{_columns.MessageTemplate}\", \"{_columns.Level}\", \"{_columns.Timestamp}\", \"{_columns.Exception}\", \"{_columns.LogEventSerialized}\" AS \"Properties\"")
            .Append(" FROM \"")
            .Append(schema)
            .Append("\".\"")
            .Append(tableName)
            .Append("\"");

        GenerateWhereClause(queryBuilder, level, searchCriteria, ref startDate, ref endDate);
        var sortClause = GenerateSortClause(sortOn, sortBy);

        queryBuilder.Append(" ORDER BY ");
        queryBuilder.Append(sortClause);
        queryBuilder.Append(" LIMIT @Count OFFSET @Offset ");

        return queryBuilder.ToString();
    }

    internal static string BuildCountLogsQuery(
        string schema,
        string tableName,
        string level,
        string searchCriteria,
        ref DateTime? startDate,
        ref DateTime? endDate)
    {
        StringBuilder queryBuilder = new();

        queryBuilder.Append($"SELECT COUNT(\"{_columns.RenderedMessage}\") FROM \"");
        queryBuilder.Append(schema);
        queryBuilder.Append("\".\"");
        queryBuilder.Append(tableName);
        queryBuilder.Append("\"");

        GenerateWhereClause(queryBuilder, level, searchCriteria, ref startDate, ref endDate);

        return queryBuilder.ToString();
    }

    private static void GenerateWhereClause(
        StringBuilder queryBuilder,
        string level,
        string searchCriteria,
        ref DateTime? startDate,
        ref DateTime? endDate)
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

        if (conditions.Count > 0)
        {
            queryBuilder
                .Append(" WHERE TRUE AND ")
                .Append(string.Join(" AND ", conditions)); ;
        }
    }

    private static string GenerateSortClause(SortProperty sortOn, SortDirection sortBy)
    {
        var isDesc = sortBy == SortDirection.Desc;

        var sortPropertyName = sortOn switch
        {
            SortProperty.Timestamp => _columns.Timestamp,
            SortProperty.Level => _columns.Level,
            SortProperty.Message => _columns.MessageTemplate,
            _ => _columns.Timestamp,
        };

        return $"\"{sortPropertyName}\" {sortBy.ToString().ToUpper()}";
    }
}