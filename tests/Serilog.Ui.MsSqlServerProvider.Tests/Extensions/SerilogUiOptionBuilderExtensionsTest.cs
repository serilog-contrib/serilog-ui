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
        private readonly ServiceCollection serviceCollection;

        public SerilogUiOptionBuilderExtensionsTest()
        {
            serviceCollection = new ServiceCollection();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("schema")]
        public void It_registers_provider_and_dependencies(string schemaName)
        {
            serviceCollection.AddSerilogUi((builder) =>
            {
                builder.UseSqlServer("https://sqlserver.example.com", "my-table", schemaName);
            });

            var serviceProvider = serviceCollection.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var provider = scope.ServiceProvider.GetService<IDataProvider>();
            provider.Should().NotBeNull().And.BeOfType<SqlServerDataProvider>();
        }

        [Fact]
        public void It_throws_on_invalid_registration()
        {
            var nullables = new List<Func<IServiceCollection>>
            {
                () => serviceCollection.AddSerilogUi((builder) => builder.UseSqlServer(null!, "name")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseSqlServer(" ", "name")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseSqlServer("", "name")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseSqlServer("name", null!)),
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
