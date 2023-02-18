using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.Web;
using System;
using System.Collections.Generic;
using Xunit;

namespace Serilog.Ui.PostgreSqlProvider.Tests.Extensions
{
    public class SerilogUiOptionBuilderExtensionsTest
    {
        private readonly ServiceCollection serviceCollection;

        public SerilogUiOptionBuilderExtensionsTest()
        {
            serviceCollection = new ServiceCollection();
        }

        [Theory]
        [InlineData(null, "public")]
        [InlineData("schema", "schema")]
        public void It_registers_provider_dependencies_with_connstring_and_tableName(string schemaName, string expected)
        {
            serviceCollection.AddSerilogUi((builder) =>
            {
                builder.UseNpgSql("https://npgsql.example.com", "my-table", schemaName);
            });
            var services = serviceCollection.BuildServiceProvider();

            services.GetRequiredService<IDataProvider>().Should().NotBeNull().And.BeOfType<PostgresDataProvider>();
            var options = services.GetRequiredService<RelationalDbOptions>();
            options.Should().NotBeNull();
            options.ConnectionString.Should().Be("https://npgsql.example.com");
            options.TableName.Should().Be("my-table");
            options.Schema.Should().Be(expected);
        }

        [Fact]
        public void It_throws_with_invalid_deps()
        {
            var nullables = new List<Func<IServiceCollection>>
            {
                () => serviceCollection.AddSerilogUi((builder) => builder.UseNpgSql(null, "name")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseNpgSql(" ", "name")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseNpgSql("", "name")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseNpgSql("name", null)),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseNpgSql("name", " ")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseNpgSql("name", "")),
            };

            foreach (var nullable in nullables)
            {
                nullable.Should().ThrowExactly<ArgumentNullException>();
            }
        }
    }
}
