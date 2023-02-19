using Ardalis.GuardClauses;
using Dapper;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using MySql.Data.MySqlClient;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.Core;
using Serilog.Ui.MySqlProvider;
using System.Threading.Tasks;

namespace MySql.Tests.Util
{
    public sealed class MySqlTestProvider : DatabaseInstance<MySqlTestcontainer, MySqlTestcontainerConfiguration>
    {
        protected override string Name => nameof(MySqlTestcontainer);

        public MySqlTestProvider()
        {
            Guard.Against.Null(configuration);
            configuration.Username = "mysql-tests";
            configuration.Database = "testdatabase";
        }

        public RelationalDbOptions DbOptions { get; set; } = new()
        {
            TableName = "Logs",
            Schema = "dbo"
        };

        protected override async Task CheckDbReadinessAsync()
        {
            DbOptions.ConnectionString = Container?.ConnectionString;

            using var dataContext = new MySqlConnection(DbOptions.ConnectionString);

            await dataContext.ExecuteAsync("SELECT 1");
        }

        protected override async Task InitializeAdditionalAsync()
        {
            var logs = LogModelFaker.Logs(100);
            Collector = new LogModelPropsCollector(logs);

            using var dataContext = new MySqlConnection(DbOptions.ConnectionString);

            await dataContext.ExecuteAsync(Costants.MySqlCreateTable);

            await dataContext.ExecuteAsync(Costants.MySqlInsertFakeData, logs);

            Provider = new MySqlDataProvider(DbOptions);
        }

    }
}
