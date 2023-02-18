using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.Web;
using System;
using System.Collections.Generic;
using Xunit;

namespace Serilog.Ui.MySqlProvider.Tests.Extensions
{
    public class SerilogUiOptionBuilderExtensionsTest
    {
        private readonly ServiceCollection serviceCollection;

        public SerilogUiOptionBuilderExtensionsTest()
        {
            serviceCollection = new ServiceCollection();
        }

        [Fact]
        public void It_registers_provider_dependencies_with_connstring_and_tableName()
        {
            serviceCollection.AddSerilogUi((builder) =>
            {
                builder.UseMySqlServer("https://mysqlserver.example.com", "my-table");
            });
            var services = serviceCollection.BuildServiceProvider();

            services.GetRequiredService<IDataProvider>().Should().NotBeNull().And.BeOfType<MySqlDataProvider>();
            var options = services.GetRequiredService<RelationalDbOptions>();
            options.Should().NotBeNull();
            options.ConnectionString.Should().Be("https://mysqlserver.example.com");
            options.TableName.Should().Be("my-table");
        }

        [Fact]
        public void It_throws_with_invalid_deps()
        {
            var nullables = new List<Func<IServiceCollection>>
            {
                () => serviceCollection.AddSerilogUi((builder) => builder.UseMySqlServer(null, "name")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseMySqlServer(" ", "name")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseMySqlServer("", "name")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseMySqlServer("name", null)),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseMySqlServer("name", " ")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseMySqlServer("name", "")),
            };

            foreach (var nullable in nullables)
            {
                nullable.Should().ThrowExactly<ArgumentNullException>();
            }
        }
    }
}
