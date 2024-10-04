using Ardalis.GuardClauses;
using Dapper;
using Microsoft.Data.Sqlite;
using Serilog;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.SqliteDataProvider;
using Serilog.Ui.SqliteDataProvider.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Sqlite.Tests.Util
{
    [CollectionDefinition(nameof(SqliteTestProvider))]
    public class SqliteCollection : ICollectionFixture<SqliteTestProvider> { }

    public sealed class SqliteTestProvider : IIntegrationRunner
    {
        private LogModelPropsCollector? _collector;

        private SqliteDataProvider? _provider;

        public SqliteTestProvider() : base()
        {
            // No need to set up a container for SQLite - using in-memory database
        }

        public SqliteDbOptions DbOptions { get; set; } = new SqliteDbOptions(string.Empty).WithTable("Logs");

        private async Task CheckDbReadinessAsync()
        {
            Guard.Against.Null(DbOptions);

            using var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            DbOptions.WithConnectionString(connection.ConnectionString);

            await connection.ExecuteAsync("SELECT 1");
        }

        private void InitializeAdditional()
        {
            var serilog = new SerilogSinkSetup(logger =>
                logger
                    .WriteTo
                    .SQLite(DbOptions.ConnectionString));
            _collector = serilog.InitializeLogs();

            _provider = new SqliteDataProvider(DbOptions, new SqliteQueryBuilder());
        }

        public IDataProvider GetDataProvider() => _provider!;

        public LogModelPropsCollector GetPropsCollector() => _collector!;

        public async Task InitializeAsync()
        {
            await CheckDbReadinessAsync();
            InitializeAdditional();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }
}
