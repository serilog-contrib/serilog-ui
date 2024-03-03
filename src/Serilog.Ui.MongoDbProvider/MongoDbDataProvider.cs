using Ardalis.GuardClauses;
using MongoDB.Driver;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            Guard.Against.Null(client, nameof(client));
            _options = options ?? throw new ArgumentNullException(nameof(options));

            _collection = client.GetDatabase(options.DatabaseName)
                .GetCollection<MongoDbLogModel>(options.CollectionName);
        }

        public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(
            int page,
            int count,
            string level = null,
            string searchCriteria = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            SortProperty sortOn = SortProperty.Timestamp,
            SortDirection sortBy = SortDirection.Desc)
        {
            var logsTask = await GetLogsAsync(page - 1, count, level, searchCriteria, startDate, endDate, sortOn, sortBy);
            var logCountTask = await CountLogsAsync(level, searchCriteria, startDate, endDate);

            return (logsTask, logCountTask);
        }

        public string Name => string.Join(".", "MongoDb", _options.DatabaseName, _options.CollectionName);

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
            try
            {
                var builder = Builders<MongoDbLogModel>.Filter.Empty;
                GenerateWhereClause(ref builder, level, searchCriteria, startDate, endDate);

                if (!string.IsNullOrWhiteSpace(searchCriteria))
                {
                    await _collection.Indexes.CreateOneAsync(
                        new CreateIndexModel<MongoDbLogModel>(Builders<MongoDbLogModel>.IndexKeys.Text(p => p.RenderedMessage)));
                }

                var sortClause = GenerateSortClause(sortOn, sortBy);

                var logs = await _collection
                    .Find(builder, new FindOptions{ Collation = new Collation("en")})
                    .Sort(sortClause)
                    .Skip(count * page)
                    .Limit(count)
                    .ToListAsync();

                var index = 1;
                foreach (var log in logs)
                    log.Id = (page * count) + index++;

                return logs.Select(log => log.ToLogModel()).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private async Task<int> CountLogsAsync(
            string level,
            string searchCriteria,
            DateTime? startDate,
            DateTime? endDate)
        {
            var builder = Builders<MongoDbLogModel>.Filter.Empty;
            GenerateWhereClause(ref builder, level, searchCriteria, startDate, endDate);

            var count = await _collection.Find(builder).CountDocumentsAsync();

            return Convert.ToInt32(count);
        }

        private static void GenerateWhereClause(
            ref FilterDefinition<MongoDbLogModel> builder,
            string level,
            string searchCriteria,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            if (!string.IsNullOrWhiteSpace(level))
                builder &= Builders<MongoDbLogModel>.Filter.Eq(entry => entry.Level, level);

            if (!string.IsNullOrWhiteSpace(searchCriteria))
                builder &= Builders<MongoDbLogModel>.Filter.Text(searchCriteria);

            if (startDate != null)
            {
                var utcStart = startDate.Value.ToUniversalTime();
                builder &= Builders<MongoDbLogModel>.Filter.Gte(entry => entry.UtcTimeStamp, utcStart);
            }

            if (endDate != null)
            {
                var utcEnd = endDate.Value.ToUniversalTime();
                builder &= Builders<MongoDbLogModel>.Filter.Lte(entry => entry.UtcTimeStamp, utcEnd);
            }
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
            
            return isDesc ?
                Builders<MongoDbLogModel>.Sort.Descending(sortPropertyName) :
                Builders<MongoDbLogModel>.Sort.Ascending(sortPropertyName);
        }
    }
}