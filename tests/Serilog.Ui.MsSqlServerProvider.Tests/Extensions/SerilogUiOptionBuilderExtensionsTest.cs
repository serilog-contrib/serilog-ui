using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.MsSqlServerProvider;
using Serilog.Ui.Web;
using System;
using System.Collections.Generic;
using Xunit;

namespace MsSql.Tests.Extensions
{
    [Trait("DI-DataProvider", "MsSql")]
    public class SerilogUiOptionBuilderExtensionsTest
    {
        private readonly ServiceCollection _serviceCollection = new();

        [Theory]
        [InlineData(null)]
        [InlineData("schema")]
        public void It_registers_provider_and_dependencies(string schemaName)
        {
            _serviceCollection.AddSerilogUi((builder) =>
            {
                builder.UseSqlServer("https://sqlserver.example.com", "my-table", schemaName);
            });

            var serviceProvider = _serviceCollection.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var provider = scope.ServiceProvider.GetService<IDataProvider>();
            provider.Should().NotBeNull().And.BeOfType<SqlServerDataProvider>();
        }

        [Fact]
        public void It_throws_on_invalid_registration()
        {
            var nullables = new List<Func<IServiceCollection>>
            {
                () => _serviceCollection.AddSerilogUi((builder) => builder.UseSqlServer(null!, "name")),
                () => _serviceCollection.AddSerilogUi((builder) => builder.UseSqlServer(" ", "name")),
                () => _serviceCollection.AddSerilogUi((builder) => builder.UseSqlServer("", "name")),
                () => _serviceCollection.AddSerilogUi((builder) => builder.UseSqlServer("name", null!)),
                () => _serviceCollection.AddSerilogUi((builder) => builder.UseSqlServer("name", " ")),
                () => _serviceCollection.AddSerilogUi((builder) => builder.UseSqlServer("name", "")),
            };

            foreach (var nullable in nullables)
            {
                nullable.Should().ThrowExactly<ArgumentNullException>();
            }
        }
    }
}
