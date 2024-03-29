﻿using FluentAssertions;
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
        private readonly ServiceCollection serviceCollection;

        public SerilogUiOptionBuilderExtensionsTest()
        {
            serviceCollection = new ServiceCollection();
        }

        [Fact]
        public void It_registers_provider_and_dependencies()
        {
            serviceCollection.AddSerilogUi((builder) =>
            {
                builder.UseMySqlServer("https://mysqlserver.example.com", "my-table");
            });
            var services = serviceCollection.BuildServiceProvider();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var provider = scope.ServiceProvider.GetService<IDataProvider>();
            provider.Should().NotBeNull().And.BeOfType<MySqlDataProvider>();
        }

        [Fact]
        public void It_throws_on_invalid_registration()
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
