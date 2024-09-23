using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Dapper;
using MySqlConnector;
using Serilog;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.Core.Models.Options;
using Serilog.Ui.MySqlProvider;
using Serilog.Ui.MySqlProvider.Extensions;
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
            DbOptions = new MySqlDbOptions("dbo").WithTable("Logs");
        }

        public MySqlDbOptions DbOptions { get; }

        protected override async Task CheckDbReadinessAsync()
        {
            Guard.Against.Null(Container);

            DbOptions.WithConnectionString((Container as MySqlContainer)?.GetConnectionString()!);

            await using var dataContext = new MySqlConnection(DbOptions.ConnectionString);

            await dataContext.ExecuteAsync("SELECT 1");
        }

        protected override Task InitializeAdditionalAsync()
        {
            SerilogSinkSetup serilog = new(logger =>
            {
                logger.WriteTo.MySQL(DbOptions.ConnectionString, batchSize: 1, storeTimestampInUtc: true);
            });

            Collector = serilog.InitializeLogs();

            Provider = new MySqlDataProvider(DbOptions, new MySqlQueryBuilder<MySqlLogModel>());

            return Task.CompletedTask;
        }
    }
}