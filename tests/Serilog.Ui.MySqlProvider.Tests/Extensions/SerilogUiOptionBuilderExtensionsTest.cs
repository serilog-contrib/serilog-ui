using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.MySqlProvider;
using Serilog.Ui.Web;
using System;
using System.Collections.Generic;
using Xunit;

namespace MySql.Tests.Extensions
{
    [Trait("DI-DataProvider", "MySql")]
    public class SerilogUiOptionBuilderExtensionsTest
    {
        private readonly ServiceCollection _serviceCollection = new();

        [Fact]
        public void It_registers_provider_and_dependencies()
        {
            _serviceCollection.AddSerilogUi((builder) =>
            {
                builder.UseMySqlServer("https://mysqlserver.example.com", "my-table");
            });
            var services = _serviceCollection.BuildServiceProvider();

            var serviceProvider = _serviceCollection.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var provider = scope.ServiceProvider.GetService<IDataProvider>();
            provider.Should().NotBeNull().And.BeOfType<MySqlDataProvider>();
        }

        [Fact]
        public void It_throws_on_invalid_registration()
        {
            var nullables = new List<Func<IServiceCollection>>
            {
                () => _serviceCollection.AddSerilogUi((builder) => builder.UseMySqlServer(null, "name")),
                () => _serviceCollection.AddSerilogUi((builder) => builder.UseMySqlServer(" ", "name")),
                () => _serviceCollection.AddSerilogUi((builder) => builder.UseMySqlServer("", "name")),
                () => _serviceCollection.AddSerilogUi((builder) => builder.UseMySqlServer("name", null)),
                () => _serviceCollection.AddSerilogUi((builder) => builder.UseMySqlServer("name", " ")),
                () => _serviceCollection.AddSerilogUi((builder) => builder.UseMySqlServer("name", "")),
            };

            foreach (var nullable in nullables)
            {
                nullable.Should().ThrowExactly<ArgumentNullException>();
            }
        }
    }
}
