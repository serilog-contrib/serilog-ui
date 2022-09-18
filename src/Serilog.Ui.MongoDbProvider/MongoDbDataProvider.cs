using MongoDB.Driver;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Serilog.Ui.MongoDbProvider
{
    public class MongoDbDataProvider : IDataProvider
    {
        private readonly IMongoCollection<MongoDbLogModel> _collection;

        public MongoDbDataProvider(IMongoClient client, MongoDbOptions options)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (options is null) throw new ArgumentNullException(nameof(options));

            _collection = client.GetDatabase(options.DatabaseName).GetCollection<MongoDbLogModel>(options.CollectionName);
            var s = _collection.CollectionNamespace;
        }

        public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(
            int page,
            int count,
            string level = null,
            string searchCriteria = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var logsTask = await GetLogsAsync(page - 1, count, level, searchCriteria, startDate, endDate);
            var logCountTask = await CountLogsAsync(level, searchCriteria, startDate, endDate);

            //await Task.WhenAll(logsTask, logCountTask);

            return (logsTask, logCountTask);
        }

        private async Task<IEnumerable<LogModel>> GetLogsAsync(
            int page,
            int count,
            string level,
            string searchCriteria,
            DateTime? startDate,
            DateTime? endDate)
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

                var logs = await _collection
                    .Find(builder)
                    .Skip(count * page)
                    .Limit(count)
                    .SortByDescending(entry => entry.Timestamp)
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

        private async Task<int> CountLogsAsync(string level, string searchCriteria, DateTime? startDate, DateTime? endDate)
        {
            var builder = Builders<MongoDbLogModel>.Filter.Empty;
            GenerateWhereClause(ref builder, level, searchCriteria, startDate, endDate);

            var count = await _collection.Find(builder).CountDocumentsAsync();

            return Convert.ToInt32(count);
        }

        private void GenerateWhereClause(
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
                builder &= Builders<MongoDbLogModel>.Filter.Gte(entry => entry.Timestamp, startDate);

            if (endDate != null)
                builder &= Builders<MongoDbLogModel>.Filter.Lte(entry => entry.Timestamp, endDate);
        }
    }
}