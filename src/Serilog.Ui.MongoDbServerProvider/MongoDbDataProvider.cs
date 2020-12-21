using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Ui.MongoDbServerProvider
{
    public class MongoDbDataProvider : IDataProvider
    {
        private readonly MongoDbOptions _options;
        private readonly IMongoClient _client;
        private readonly IMongoCollection<MongoDbLogModel> collection;

        public MongoDbDataProvider(
            IMongoClient client,
            MongoDbOptions options)
        {
            this._options = options;
            this._client = client;

            this.collection = client.GetDatabase(_options.DatabaseName).GetCollection<MongoDbLogModel>(_options.CollectionName);
        }

        public async Task<(IEnumerable<LogModel>, int)> FetchDataAsync(
            int page,
            int count,
            string level = null,
            string searchCriteria = null)
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
            var result = new List<LogModel>();

            var builder = Builders<MongoDbLogModel>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(level))
                //builder &= Builders<MongoDbLogModel>.Filter.Eq("level", level);
                builder &= Builders<MongoDbLogModel>.Filter.Eq(entry => entry.Level, level);

            if (!string.IsNullOrWhiteSpace(searchCriteria))
                builder &= Builders<MongoDbLogModel>.Filter.Text(searchCriteria);

            var logs = await collection.Find(builder)
                .Skip(count * page)
                .Limit(count)
                .SortByDescending(entry => entry.Id)
                .ToListAsync();

            foreach (var log in logs)
                result.Add(log.ToLogModel());

            return result;
        }

        private async Task<int> CountLogsAsync(string level, string searchCriteria)
        {
            var builder = Builders<MongoDbLogModel>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(level))
                //builder &= Builders<MongoDbLogModel>.Filter.Eq("level", level);
                builder &= Builders<MongoDbLogModel>.Filter.Eq(entry => entry.Level, level);

            if (!string.IsNullOrWhiteSpace(searchCriteria))
                builder &= Builders<MongoDbLogModel>.Filter.Text(searchCriteria);

            var count = await collection.Find(builder)
                .CountDocumentsAsync();

            return Convert.ToInt32(count);
        }
    }
}
