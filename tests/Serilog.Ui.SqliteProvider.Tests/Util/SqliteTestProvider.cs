using Ardalis.GuardClauses;
using Dapper;
using Microsoft.Data.Sqlite;
using Serilog;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.SqliteDataProvider;
using Serilog.Ui.SqliteDataProvider.Extensions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sqlite.Tests.Util
{
    [CollectionDefinition(nameof(SqliteTestProvider))]
    public class SqliteCollection : ICollectionFixture<SqliteTestProvider> { }

    public sealed class SqliteTestProvider : IIntegrationRunner
    {
        private static string DbName() => $"integration-{DateTime.UtcNow:O}.db".Replace(':', '-');
        private string _dbInstanceName = string.Empty;

        private LogModelPropsCollector? _collector;

        private SqliteDataProvider? _provider;

        public SqliteTestProvider() : base()
        {
            // No need to set up a container for SQLite - using in-memory database
        }

        public SqliteDbOptions DbOptions { get; set; } = new SqliteDbOptions().WithTable("Logs");
        private async Task CheckDbReadinessAsync()
        {
            Guard.Against.Null(DbOptions);

            _dbInstanceName = DbName();
            DbOptions.WithConnectionString($"Data Source={_dbInstanceName}");

            using var connection = new SqliteConnection(DbOptions.ConnectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync("SELECT 1");

            InitializeAdditional();
        }

        private void InitializeAdditional()
        {
            var serilog = new SerilogSinkSetup(logger =>
                logger
                    .WriteTo
                    .SQLite(_dbInstanceName, batchSize: 1, storeTimestampInUtc: true));
            _collector = serilog.InitializeLogs();

            _provider = new SqliteDataProvider(DbOptions, new SqliteQueryBuilder());
        }

        public IDataProvider GetDataProvider() => _provider!;

        public LogModelPropsCollector GetPropsCollector() => _collector!;

        public Task InitializeAsync()
        {
            return CheckDbReadinessAsync();
        }

        public Task DisposeAsync() => Task.CompletedTask;

        public void Dispose() { }
    }
}
