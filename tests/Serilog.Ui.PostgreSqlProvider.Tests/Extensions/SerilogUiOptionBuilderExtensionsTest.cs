using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.PostgreSqlProvider;
using Serilog.Ui.Web.Extensions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Postgres.Tests.Extensions
{
    [Trait("DI-DataProvider", "Postgres")]
    public class SerilogUiOptionBuilderExtensionsTest
    {
        private readonly ServiceCollection _serviceCollection = new();

        [Theory]
        [InlineData(null)]
        [InlineData("schema")]
        public void It_registers_provider_and_dependencies(string? schemaName)
        {
            _serviceCollection.AddSerilogUi((builder) =>
            {
                builder.UseNpgSql(PostgreSqlSinkType.SerilogSinksPostgreSQLAlternative, "https://npgsql.example.com", "my-table", schemaName);
            });

            var serviceProvider = _serviceCollection.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var provider = scope.ServiceProvider.GetService<IDataProvider>();
            provider.Should().NotBeNull().And.BeOfType<PostgresDataProvider>();
        }

        [Fact]
        public void It_throws_on_invalid_registration()
        {
            var nullables = new List<Func<IServiceCollection>>
            {
                () => _serviceCollection.AddSerilogUi((builder) => builder.UseNpgSql(PostgreSqlSinkType.SerilogSinksPostgreSQLAlternative, null, "name")),
                () => _serviceCollection.AddSerilogUi((builder) => builder.UseNpgSql(PostgreSqlSinkType.SerilogSinksPostgreSQLAlternative," ", "name")),
                () => _serviceCollection.AddSerilogUi((builder) => builder.UseNpgSql(PostgreSqlSinkType.SerilogSinksPostgreSQLAlternative,"", "name")),
                () => _serviceCollection.AddSerilogUi((builder) => builder.UseNpgSql(PostgreSqlSinkType.SerilogSinksPostgreSQLAlternative,"name", null)),
                () => _serviceCollection.AddSerilogUi((builder) => builder.UseNpgSql(PostgreSqlSinkType.SerilogSinksPostgreSQLAlternative,"name", " ")),
                () => _serviceCollection.AddSerilogUi((builder) => builder.UseNpgSql(PostgreSqlSinkType.SerilogSinksPostgreSQLAlternative,"name", "")),
            };

            foreach (var nullable in nullables)
            {
                nullable.Should().ThrowExactly<ArgumentNullException>();
            }
        }
    }
}