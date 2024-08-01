using Ardalis.GuardClauses;
using Dapper;
using Microsoft.Data.Sqlite;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.Core;
using Serilog.Ui.SqliteDataProvider;
using System.Threading.Tasks;
using Xunit;

namespace MySql.Tests.Util
{
    [CollectionDefinition(nameof(SqliteDataProvider))]
    public class MySqlCollection : ICollectionFixture<SqliteTestProvider> { }

    public sealed class SqliteTestProvider : DatabaseInstance
    {
        protected override string Name => "SqliteInMemory";

        public SqliteTestProvider() : base()
        {
            // No need to set up a container for SQLite - using in-memory database
        }

        public RelationalDbOptions DbOptions { get; set; } = new()
        {
            TableName = "Logs",
            Schema = "dbo"
        };

        protected override async Task CheckDbReadinessAsync()
        {
            Guard.Against.Null(DbOptions);

            using var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            DbOptions.ConnectionString = connection.ConnectionString;

            await connection.ExecuteAsync("SELECT 1");
        }

        protected override async Task InitializeAdditionalAsync()
        {
            var logs = LogModelFaker.Logs(100);
            Collector = new LogModelPropsCollector(logs);

            using var connection = new SqliteConnection(DbOptions.ConnectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(Costants.SqliteCreateTable);

            await connection.ExecuteAsync(Costants.SqliteInsertFakeData, logs);

            Provider = new SqliteDataProvider(DbOptions); // Update this if needed for SQLite
        }
    }
}
