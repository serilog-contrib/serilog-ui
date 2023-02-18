using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.Web;
using System;
using System.Collections.Generic;
using Xunit;

namespace Serilog.Ui.MsSqlServerProvider.Tests.Extensions
{
    public class SerilogUiOptionBuilderExtensionsTest
    {
        private readonly ServiceCollection serviceCollection;

        public SerilogUiOptionBuilderExtensionsTest()
        {
            serviceCollection = new ServiceCollection();
        }

        [Theory]
        [InlineData(null, "dbo")]
        [InlineData("schema", "schema")]
        public void It_registers_provider_dependencies_with_connstring_and_tableName(string schemaName, string expected)
        {
            serviceCollection.AddSerilogUi((builder) =>
            {
                builder.UseSqlServer("https://sqlserver.example.com", "my-table", schemaName);
            });
            var services = serviceCollection.BuildServiceProvider();

            services.GetRequiredService<IDataProvider>().Should().NotBeNull().And.BeOfType<SqlServerDataProvider>();
            var options = services.GetRequiredService<RelationalDbOptions>();
            options.Should().NotBeNull();
            options.ConnectionString.Should().Be("https://sqlserver.example.com");
            options.TableName.Should().Be("my-table");
            options.Schema.Should().Be(expected);
        }

        [Fact]
        public void It_throws_with_invalid_deps()
        {
            var nullables = new List<Func<IServiceCollection>>
            {
                () => serviceCollection.AddSerilogUi((builder) => builder.UseSqlServer(null, "name")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseSqlServer(" ", "name")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseSqlServer("", "name")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseSqlServer("name", null)),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseSqlServer("name", " ")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseSqlServer("name", "")),
            };

            foreach (var nullable in nullables)
            {
                nullable.Should().ThrowExactly<ArgumentNullException>();
            }
        }
    }
}
