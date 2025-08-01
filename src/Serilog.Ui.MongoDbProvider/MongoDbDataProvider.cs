using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MongoDB.Driver;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;
using static Serilog.Ui.Core.Models.SearchOptions;
using SortDirection = Serilog.Ui.Core.Models.SearchOptions.SortDirection;

namespace Serilog.Ui.MongoDbProvider;

public class MongoDbDataProvider : IDataProvider
{
    private readonly IMongoCollection<MongoDbLogModel> _collection;

    private readonly MongoDbOptions _options;

    public MongoDbDataProvider(IMongoClient client, MongoDbOptions options)
    {
        Guard.Against.Null(client);
        _options = Guard.Against.Null(options);

        _collection = client.GetDatabase(options.DatabaseName)
            .GetCollection<MongoDbLogModel>(options.CollectionName);
    }

    public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams,
        CancellationToken cancellationToken = default)
    {
        queryParams.ToUtcDates();

        IEnumerable<LogModel>? logsTask = await GetLogsAsync(queryParams, cancellationToken);
        int logCountTask = await CountLogsAsync(queryParams);

        return (logsTask, logCountTask);
    }

    public Task<DashboardModel> FetchDashboardAsync(CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();

    public string Name => _options.ProviderName;

    private async Task<IEnumerable<LogModel>> GetLogsAsync(FetchLogsQuery queryParams,
        CancellationToken cancellationToken = default)
    {
        try
        {
            FilterDefinition<MongoDbLogModel>? builder = Builders<MongoDbLogModel>.Filter.Empty;
            GenerateWhereClause(ref builder, queryParams);

            if (!string.IsNullOrWhiteSpace(queryParams.SearchCriteria))
                await _collection.Indexes.CreateOneAsync(
                    new CreateIndexModel<MongoDbLogModel>(
                        Builders<MongoDbLogModel>.IndexKeys.Text(p => p.RenderedMessage)),
                    cancellationToken: cancellationToken);

            SortDefinition<MongoDbLogModel> sortClause = GenerateSortClause(queryParams.SortOn, queryParams.SortBy);

            int rowNoStart = queryParams.Count * queryParams.Page;

            List<MongoDbLogModel>? logs = await _collection
                .Find(builder, new FindOptions { Collation = new Collation("en") })
                .Sort(sortClause)
                .Skip(rowNoStart)
                .Limit(queryParams.Count)
                .ToListAsync(cancellationToken);

            return logs.Select((item, i) => item.ToLogModel(rowNoStart, i)).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    private async Task<int> CountLogsAsync(FetchLogsQuery queryParams)
    {
        FilterDefinition<MongoDbLogModel>? builder = Builders<MongoDbLogModel>.Filter.Empty;
        GenerateWhereClause(ref builder, queryParams);

        long count = await _collection.Find(builder).CountDocumentsAsync();

        return Convert.ToInt32(count);
    }

    private static void GenerateWhereClause(ref FilterDefinition<MongoDbLogModel> builder, FetchLogsQuery queryParams)
    {
        if (!string.IsNullOrWhiteSpace(queryParams.Level))
            builder &= Builders<MongoDbLogModel>.Filter.Eq(entry => entry.Level, queryParams.Level);

        if (!string.IsNullOrWhiteSpace(queryParams.SearchCriteria))
            builder &= Builders<MongoDbLogModel>.Filter.Text(queryParams.SearchCriteria);

        if (queryParams.StartDate != null)
        {
            DateTime? utcStart = queryParams.StartDate;
            builder &= Builders<MongoDbLogModel>.Filter.Gte(entry => entry.UtcTimeStamp, utcStart);
        }

        if (queryParams.EndDate == null) return;

        DateTime? utcEnd = queryParams.EndDate;
        builder &= Builders<MongoDbLogModel>.Filter.Lte(entry => entry.UtcTimeStamp, utcEnd);
    }

    private static SortDefinition<MongoDbLogModel> GenerateSortClause(SortProperty sortOn, SortDirection sortBy)
    {
        bool isDesc = sortBy == SortDirection.Desc;

        // workaround to use utc timestamp
        string? sortPropertyName = sortOn switch
        {
            SortProperty.Level => typeof(MongoDbLogModel).GetProperty(sortOn.ToString())?.Name ?? string.Empty,
            SortProperty.Message => nameof(MongoDbLogModel.RenderedMessage),
            SortProperty.Timestamp => nameof(MongoDbLogModel.UtcTimeStamp),
            _ => nameof(MongoDbLogModel.UtcTimeStamp)
        };

        return isDesc
            ? Builders<MongoDbLogModel>.Sort.Descending(sortPropertyName)
            : Builders<MongoDbLogModel>.Sort.Ascending(sortPropertyName);
    }
}