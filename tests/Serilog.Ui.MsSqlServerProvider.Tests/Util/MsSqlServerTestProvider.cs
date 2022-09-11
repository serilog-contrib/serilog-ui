using Dapper;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Data.SqlClient;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.Core;
using System.Threading.Tasks;

namespace Serilog.Ui.MsSqlServerProvider.Tests.Util
{
    public sealed class MsSqlServerTestProvider : DatabaseInstance<MsSqlTestcontainer, MsSqlTestcontainerConfiguration>
    {
        public RelationalDbOptions DbOptions { get; set; } = new()
        {
            TableName = "Logs",
            Schema = "dbo"
        };

        protected override async Task CheckDbReadinessAsync()
        {
            DbOptions.ConnectionString = Container?.ConnectionString + "TrustServerCertificate=True;";

            using var dataContext = new SqlConnection(DbOptions.ConnectionString);
            
            await dataContext.ExecuteAsync("SELECT DATABASEPROPERTYEX(N'master', 'Collation')");
        }

        protected override async Task InitializeAdditionalAsync()
        {
            using var dataContext = new SqlConnection(DbOptions.ConnectionString);

            await dataContext.ExecuteAsync(Costants.MsSqlCreateTable);

            await dataContext.ExecuteAsync(Costants.MsSqlInsertFakeData, LogModelFaker.Logs());

            Provider = new SqlServerDataProvider(DbOptions);
        }
    }
}
