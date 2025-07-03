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

namespace Serilog.Ui.MongoDbProvider
{
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

        public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams, CancellationToken cancellationToken = default)
        {
            queryParams.ToUtcDates();

            var logsTask = await GetLogsAsync(queryParams, cancellationToken);
            var logCountTask = await CountLogsAsync(queryParams);

            return (logsTask, logCountTask);
        }

        public string Name => _options.ProviderName;

        private async Task<IEnumerable<LogModel>> GetLogsAsync(FetchLogsQuery queryParams, CancellationToken cancellationToken = default)
        {
            try
            {
                var builder = Builders<MongoDbLogModel>.Filter.Empty;
                GenerateWhereClause(ref builder, queryParams);

                if (!string.IsNullOrWhiteSpace(queryParams.SearchCriteria))
                {
                    await _collection.Indexes.CreateOneAsync(
                        new CreateIndexModel<MongoDbLogModel>(Builders<MongoDbLogModel>.IndexKeys.Text(p => p.RenderedMessage)),
                        cancellationToken: cancellationToken);
                }

                var sortClause = GenerateSortClause(queryParams.SortOn, queryParams.SortBy);

                var rowNoStart = queryParams.Count * queryParams.Page;

                var logs = await _collection
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
            var builder = Builders<MongoDbLogModel>.Filter.Empty;
            GenerateWhereClause(ref builder, queryParams);

            var count = await _collection.Find(builder).CountDocumentsAsync();

            return Convert.ToInt32(count);
        }

        private static void GenerateWhereClause(
            ref FilterDefinition<MongoDbLogModel> builder,
            FetchLogsQuery queryParams)
        {
            if (!string.IsNullOrWhiteSpace(queryParams.Level))
                builder &= Builders<MongoDbLogModel>.Filter.Eq(entry => entry.Level, queryParams.Level);

            if (!string.IsNullOrWhiteSpace(queryParams.SearchCriteria))
                builder &= Builders<MongoDbLogModel>.Filter.Text(queryParams.SearchCriteria);

            if (queryParams.StartDate != null)
            {
                var utcStart = queryParams.StartDate;
                builder &= Builders<MongoDbLogModel>.Filter.Gte(entry => entry.UtcTimeStamp, utcStart);
            }

            if (queryParams.EndDate == null) return;

            var utcEnd = queryParams.EndDate;
            builder &= Builders<MongoDbLogModel>.Filter.Lte(entry => entry.UtcTimeStamp, utcEnd);
        }


        private static SortDefinition<MongoDbLogModel> GenerateSortClause(SortProperty sortOn, SortDirection sortBy)
        {
            var isDesc = sortBy == SortDirection.Desc;

            // workaround to use utc timestamp
            var sortPropertyName = sortOn switch
            {
                SortProperty.Level => typeof(MongoDbLogModel).GetProperty(sortOn.ToString())?.Name ?? string.Empty,
                SortProperty.Message => nameof(MongoDbLogModel.RenderedMessage),
                SortProperty.Timestamp => nameof(MongoDbLogModel.UtcTimeStamp),
                _ => nameof(MongoDbLogModel.UtcTimeStamp)
            };

            return isDesc ? Builders<MongoDbLogModel>.Sort.Descending(sortPropertyName) : Builders<MongoDbLogModel>.Sort.Ascending(sortPropertyName);
        }

        public async Task<DashboardModel> FetchDashboardAsync(CancellationToken cancellationToken = default)
        {
            var dashboard = new DashboardModel();
            var today = DateTime.Today.ToUniversalTime();
            var tomorrow = today.AddDays(1);

            // Get total logs count
            dashboard.TotalLogs = Convert.ToInt32(await _collection.CountDocumentsAsync(Builders<MongoDbLogModel>.Filter.Empty, cancellationToken: cancellationToken));

            // Get logs count by level
            var levelCounts = await _collection.Aggregate()
                .Group(x => x.Level, g => new { Level = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);
            dashboard.LogsByLevel = levelCounts.ToDictionary(x => x.Level ?? "Unknown", x => x.Count);

            // Get today's logs count
            var todayFilter = Builders<MongoDbLogModel>.Filter.And(
                Builders<MongoDbLogModel>.Filter.Gte(x => x.UtcTimeStamp, today),
                Builders<MongoDbLogModel>.Filter.Lt(x => x.UtcTimeStamp, tomorrow)
            );
            dashboard.TodayLogs = Convert.ToInt32(await _collection.CountDocumentsAsync(todayFilter, cancellationToken: cancellationToken));

            // Get today's error logs count
            var todayErrorFilter = Builders<MongoDbLogModel>.Filter.And(
                Builders<MongoDbLogModel>.Filter.Eq(x => x.Level, "Error"),
                Builders<MongoDbLogModel>.Filter.Gte(x => x.UtcTimeStamp, today),
                Builders<MongoDbLogModel>.Filter.Lt(x => x.UtcTimeStamp, tomorrow)
            );
            dashboard.TodayErrorLogs = Convert.ToInt32(await _collection.CountDocumentsAsync(todayErrorFilter, cancellationToken: cancellationToken));

            return dashboard;
        }
    }
}
