using Ardalis.GuardClauses;
using Dapper;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Npgsql;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.Core;
using System.Linq;
using System.Threading.Tasks;

namespace Serilog.Ui.PostgreSqlProvider.Tests.Util
{
    public sealed class PostgresTestProvider : DatabaseInstance<PostgreSqlTestcontainer, PostgreSqlTestcontainerConfiguration>
    {
        protected override string Name => nameof(PostgreSqlTestcontainer);

        public PostgresTestProvider()
        {
            Guard.Against.Null(configuration);
            configuration.Username = "mysql-tests";
            configuration.Database = "testdatabase";
        }

        public RelationalDbOptions DbOptions { get; set; } = new()
        {
            TableName = "logs",
            Schema = "public"
        };

        protected override async Task CheckDbReadinessAsync()
        {
            DbOptions.ConnectionString = Container?.ConnectionString ?? string.Empty;

            using var dataContext = new NpgsqlConnection(DbOptions.ConnectionString);

            await dataContext.ExecuteAsync("SELECT 1");
        }

        protected override async Task InitializeAdditionalAsync()
        {
            var logs = LogModelFaker.Logs(100);
            
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
