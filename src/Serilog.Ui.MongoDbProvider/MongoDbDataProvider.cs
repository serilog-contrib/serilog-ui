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
        }

        public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(
            int page,
            int count,
            string level = null,
            string searchCriteria = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var logsTask = GetLogsAsync(page - 1, count, level, searchCriteria);
            var logCountTask = CountLogsAsync(level, searchCriteria);

            await Task.WhenAll(logsTask, logCountTask);

            return (await logsTask, await logCountTask);
        }

        private async Task<IEnumerable<LogModel>> GetLogsAsync(
            int page,
            int count,
            string level,
            string searchCriteria)
        {
            var builder = Builders<MongoDbLogModel>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(level))
                builder &= Builders<MongoDbLogModel>.Filter.Eq(entry => entry.Level, level);

            if (!string.IsNullOrWhiteSpace(searchCriteria))
                builder &= Builders<MongoDbLogModel>.Filter.Text(searchCriteria);

            var logs = await _collection.Find(builder)
                .Skip(count * page)
                .Limit(count)
                .SortByDescending(entry => entry.Timestamp)
                .ToListAsync();

            var index = 1;
            foreach (var log in logs)
                log.Id = (page * count) + index++;

            return logs.Select(log => log.ToLogModel()).ToList();
        }

        private async Task<int> CountLogsAsync(string level, string searchCriteria)
        {
            var builder = Builders<MongoDbLogModel>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(level))
                builder &= Builders<MongoDbLogModel>.Filter.Eq(entry => entry.Level, level);

            if (!string.IsNullOrWhiteSpace(searchCriteria))
                builder &= Builders<MongoDbLogModel>.Filter.Text(searchCriteria);

            var count = await _collection.Find(builder)
                .CountDocumentsAsync();

            return Convert.ToInt32(count);
        }
    }
}