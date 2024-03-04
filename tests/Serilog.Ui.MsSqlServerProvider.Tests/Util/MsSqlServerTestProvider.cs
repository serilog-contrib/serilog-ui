using Dapper;
using Testcontainers.MsSql;
using Microsoft.Data.SqlClient;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.Core;
using Serilog.Ui.MsSqlServerProvider;
using System.Threading.Tasks;
using Xunit;

namespace MsSql.Tests.Util
{
    [CollectionDefinition(nameof(SqlServerDataProvider))]
    public class SqlServerCollection : ICollectionFixture<MsSqlServerTestProvider> { }

    public sealed class MsSqlServerTestProvider : DatabaseInstance
    {
        public MsSqlServerTestProvider()
        {
            Container = new MsSqlBuilder().Build();
        }

        public RelationalDbOptions DbOptions { get; set; } = new()
        {
            TableName = "Logs",
            Schema = "dbo"
        };

        protected override string Name => nameof(MsSqlContainer);

        protected override async Task CheckDbReadinessAsync()
        {
            DbOptions.ConnectionString = (Container as MsSqlContainer)?.GetConnectionString();

            await using var dataContext = new SqlConnection(DbOptions.ConnectionString);

            await dataContext.ExecuteAsync("SELECT DATABASEPROPERTYEX(N'master', 'Collation')");
        }

        protected override async Task InitializeAdditionalAsync()
        {
            var logs = LogModelFaker.Logs(100);
            Collector = new LogModelPropsCollector(logs);

            await using var dataContext = new SqlConnection(DbOptions.ConnectionString);

            await dataContext.ExecuteAsync(Costants.MsSqlCreateTable);

            await dataContext.ExecuteAsync(Costants.MsSqlInsertFakeData, logs);

            SqlMapper.AddTypeHandler(new DapperDateTimeHandler());
            Provider = new SqlServerDataProvider(DbOptions);
        }
    }
}
