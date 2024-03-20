using Ardalis.GuardClauses;
using Dapper;
using MySqlConnector;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.MySqlProvider;
using System.Threading.Tasks;
using Serilog;
using Serilog.Ui.Core.OptionsBuilder;
using Testcontainers.MySql;
using Xunit;

namespace MySql.Tests.Util
{
    [CollectionDefinition(nameof(MySqlDataProvider))]
    public class MySqlCollection : ICollectionFixture<MySqlTestProvider>
    {
    }

    public sealed class MySqlTestProvider : DatabaseInstance
    {
        protected override string Name => nameof(MySqlContainer);

        public MySqlTestProvider()
        {
            Container = new MySqlBuilder().Build();
        }

        public RelationalDbOptions DbOptions { get; set; } = new RelationalDbOptions("dbo").WithTable("Logs");

        protected override async Task CheckDbReadinessAsync()
        {
            Guard.Against.Null(Container);

            DbOptions.WithConnectionString((Container as MySqlContainer)?.GetConnectionString());

            await using var dataContext = new MySqlConnection(DbOptions.ConnectionString);

            await dataContext.ExecuteAsync("SELECT 1");
        }

        protected override Task InitializeAdditionalAsync()
        {
            var serilog = new SerilogSinkSetup(logger =>
            {
                logger.WriteTo.MySQL(DbOptions.ConnectionString, batchSize: 1, storeTimestampInUtc: true);
            });
            Collector = serilog.InitializeLogs();

            Provider = new MySqlDataProvider(DbOptions);

            return Task.CompletedTask;
        }
    }
}