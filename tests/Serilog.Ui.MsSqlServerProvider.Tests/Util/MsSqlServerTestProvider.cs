using Dapper;
using Testcontainers.MsSql;
using Microsoft.Data.SqlClient;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.MsSqlServerProvider;
using System.Threading.Tasks;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.Ui.Core.OptionsBuilder;
using Xunit;

namespace MsSql.Tests.Util
{
    [CollectionDefinition(nameof(SqlServerDataProvider))]
    public class SqlServerCollection : ICollectionFixture<MsSqlServerTestProvider>
    {
    }

    public sealed class MsSqlServerTestProvider : DatabaseInstance
    {
        public MsSqlServerTestProvider()
        {
            Container = new MsSqlBuilder().Build();
        }

        public RelationalDbOptions DbOptions { get; set; } = new RelationalDbOptions("dbo").WithTable("Logs");

        protected override string Name => nameof(MsSqlContainer);

        protected override async Task CheckDbReadinessAsync()
        {
            DbOptions.WithConnectionString((Container as MsSqlContainer)?.GetConnectionString());

            await using var dataContext = new SqlConnection(DbOptions.ConnectionString);

            await dataContext.ExecuteAsync("SELECT DATABASEPROPERTYEX(N'master', 'Collation')");
        }

        protected override Task InitializeAdditionalAsync()
        {
            var serilog = new SerilogSinkSetup(logger =>
            {
                logger.WriteTo.MSSqlServer(DbOptions.ConnectionString,
                    new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true });
            });
            Collector = serilog.InitializeLogs();

            SqlMapper.AddTypeHandler(new DapperDateTimeHandler());
            Provider = new SqlServerDataProvider(DbOptions);

            return Task.CompletedTask;
        }
    }
}