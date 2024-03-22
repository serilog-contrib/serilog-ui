using Dapper;
using Npgsql;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.PostgreSqlProvider;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using Testcontainers.PostgreSql;
using Xunit;

namespace Postgres.Tests.Util
{
    [CollectionDefinition(nameof(PostgresDataProvider))]
    public class PostgresCollection : ICollectionFixture<PostgresTestProvider>
    { } 

    public sealed class PostgresTestProvider : DatabaseInstance
    {
        protected override string Name => nameof(PostgreSqlContainer);

        public PostgresTestProvider()
        {
            Container = new PostgreSqlBuilder().Build();
            QueryBuilder.SetSinkType(PostgreSqlSinkType.SerilogSinksPostgreSQLAlternative);
        }

        public PostgreSqlDbOptions DbOptions { get; set; } = new()
        {
            TableName = "logs",
            Schema = "public",
            SinkType = PostgreSqlSinkType.SerilogSinksPostgreSQLAlternative
        };

        protected override async Task CheckDbReadinessAsync()
        {
            DbOptions.ConnectionString = (Container as PostgreSqlContainer)?.GetConnectionString() ?? string.Empty;

            await using var dataContext = new NpgsqlConnection(DbOptions.ConnectionString);

            await dataContext.ExecuteAsync("SELECT 1");
        }

        protected override  Task InitializeAdditionalAsync()
        {
            var serilog = new SerilogSinkSetup(logger =>
            {
                logger.WriteTo.PostgreSQL(DbOptions.ConnectionString, "logs", null, LogEventLevel.Verbose, schemaName: "public", needAutoCreateTable: true, batchSizeLimit: 1);
            });

            Collector = serilog.InitializeLogs();

            Provider = new PostgresDataProvider(DbOptions);
            return Task.CompletedTask;
        }
    }
}