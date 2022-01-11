using MongoDB.Driver;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Serilog.Ui.Core.Models.SearchOptions;

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
            DateTime? endDate = null,
            SortProperty sortOn = SortProperty.Timestamp,
            Core.Models.SearchOptions.SortDirection sortBy = Core.Models.SearchOptions.SortDirection.Desc)
        {
            var logsTask = await GetLogsAsync(page - 1, count, level, searchCriteria, startDate, endDate, sortOn, sortBy);
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
            DateTime? endDate,
            SortProperty sortOn = SortProperty.Timestamp,
            Core.Models.SearchOptions.SortDirection sortBy = Core.Models.SearchOptions.SortDirection.Desc)
        {
            try
            {
                var builder = Builders<MongoDbLogModel>.Filter.Empty;
                GenerateWhereClause(ref builder, level, searchCriteria, startDate, endDate);
                var logsFind = _collection
                    .Find(builder);

                var isDesc = sortBy == Core.Models.SearchOptions.SortDirection.Desc;
                var sortPropertyName = typeof(MongoDbLogModel).GetProperty(sortOn.ToString()).Name;
                // workaround to use utctimestamp
                if (sortPropertyName.Equals(SortProperty.Timestamp.ToString())) sortPropertyName = nameof(MongoDbLogModel.UtcTimeStamp);
                if (sortPropertyName.Equals(SortProperty.Message.ToString())) sortPropertyName = nameof(MongoDbLogModel.RenderedMessage);
                SortDefinition<MongoDbLogModel> sortBuilder = isDesc ?
                    Builders<MongoDbLogModel>.Sort.Descending(sortPropertyName) :
                    Builders<MongoDbLogModel>.Sort.Ascending(sortPropertyName);
                var logs = await
                    logsFind.Sort(sortBuilder)
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