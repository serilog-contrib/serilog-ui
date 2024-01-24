using Dapper;
//using 
using Npgsql;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.Core;
using Serilog.Ui.PostgreSqlProvider;
using System.Linq;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Xunit;

namespace Postgres.Tests.Util
{
    [CollectionDefinition(nameof(PostgresDataProvider))]
    public class PostgresCollection : ICollectionFixture<PostgresTestProvider> { }
    public sealed class PostgresTestProvider : DatabaseInstance
    {
        protected override string Name => nameof(PostgreSqlContainer);

        public PostgresTestProvider()
        {
            Container = new PostgreSqlBuilder().Build();
        }

        public RelationalDbOptions DbOptions { get; set; } = new()
        {
            TableName = "logs",
            Schema = "public"
        };

        protected override async Task CheckDbReadinessAsync()
        {
            DbOptions.ConnectionString = (Container as PostgreSqlContainer)?.GetConnectionString() ?? string.Empty;

            using var dataContext = new NpgsqlConnection(DbOptions.ConnectionString);

            await dataContext.ExecuteAsync("SELECT 1");
        }

        protected override async Task InitializeAdditionalAsync()
        {
            var logs = LogModelFaker.Logs(100)
                .ToList();

            // manual conversion due to current implementation, based on a INT level column
            var postgresTableLogs = logs.Select(p => new
            {
                p.RowNo,
                Level = LogLevelConverter.GetLevelValue(p.Level),
                p.Message,
                p.Exception,
                p.PropertyType,
                p.Properties,
                p.Timestamp,
            });

            Collector = new LogModelPropsCollector(logs);

            using var dataContext = new NpgsqlConnection(DbOptions.ConnectionString);

            await dataContext.ExecuteAsync(Costants.PostgresCreateTable);

            await dataContext.ExecuteAsync(Costants.PostgresInsertFakeData, postgresTableLogs);

            Provider = new PostgresDataProvider(DbOptions);
        }

    }
}
