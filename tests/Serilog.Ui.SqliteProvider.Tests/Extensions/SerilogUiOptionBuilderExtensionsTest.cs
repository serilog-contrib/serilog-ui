using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.Core.Models.Options;
using Serilog.Ui.SqliteDataProvider;
using Serilog.Ui.SqliteDataProvider.Extensions;
using Serilog.Ui.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sqlite.Tests.Extensions
{
    [Trait("DI-DataProvider", "Sqlite")]
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
            serviceCollection.AddSerilogUi(builder =>
            {
                builder.UseSqliteServer(opt => opt.WithConnectionString("https://sqliteserver.example.com").WithTable("my-table"));
            });
            var services = serviceCollection.BuildServiceProvider();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var provider = scope.ServiceProvider.GetService<IDataProvider>();
            provider.Should().NotBeNull().And.BeOfType<SqliteDataProvider>();
        }

        [Fact]
        public void It_registers_multiple_providers()
        {
            serviceCollection.AddSerilogUi(builder =>
            {
                builder.UseSqliteServer(opt => opt.WithConnectionString("https://sqliteserver.example.com").WithTable("my-table"));
                builder.UseSqliteServer(opt => opt.WithConnectionString("https://sqliteserver2.example.com").WithTable("my-table2"));
            });

            var serviceProvider = serviceCollection.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var providers = scope.ServiceProvider.GetServices<IDataProvider>().ToList();
            providers.Should().HaveCount(2).And.AllBeOfType<SqliteDataProvider>();
            providers.Select(p => p.Name).Should().OnlyHaveUniqueItems();

            var providersOptions = serviceProvider.GetRequiredService<ProvidersOptions>();
            providersOptions.DisabledSortProviderNames.Should().BeEmpty();
            providersOptions.ExceptionAsStringProviderNames.Should().HaveCount(2);
        }

        [Fact]
        public void It_throws_on_invalid_registration()
        {
            var nullables = new List<Func<IServiceCollection>>
            {
                () => serviceCollection.AddSerilogUi((builder) => builder.UseSqliteServer(_ => {})),
                () => serviceCollection.AddSerilogUi(builder => builder.UseSqliteServer(opt =>
                    opt.WithConnectionString(null!).WithTable("my-table"))),
                () => serviceCollection.AddSerilogUi(builder => builder.UseSqliteServer(opt =>
                    opt.WithConnectionString(" ").WithTable("my-table"))),
                () => serviceCollection.AddSerilogUi(builder => builder.UseSqliteServer(opt =>
                    opt.WithConnectionString(string.Empty).WithTable("my-table"))),
                () => serviceCollection.AddSerilogUi(builder => builder.UseSqliteServer(opt =>
                    opt.WithConnectionString("name").WithTable(null!))),
                () => serviceCollection.AddSerilogUi(builder => builder.UseSqliteServer(opt =>
                    opt.WithConnectionString("name").WithTable(" "))),
                () => serviceCollection.AddSerilogUi(builder => builder.UseSqliteServer(opt =>
                    opt.WithConnectionString("name").WithTable(string.Empty))),
                // if user sets an invalid schema, default value will be overridden an validation should fail
                () => serviceCollection.AddSerilogUi(builder =>
                    builder.UseSqliteServer(opt => opt.WithConnectionString("conn").WithTable("ok").WithSchema(null!))),
                () => serviceCollection.AddSerilogUi(builder =>
                    builder.UseSqliteServer(opt => opt.WithConnectionString("conn").WithTable("ok").WithSchema(" "))),
                () => serviceCollection.AddSerilogUi(builder =>
                    builder.UseSqliteServer(opt => opt.WithConnectionString("conn").WithTable("ok").WithSchema(string.Empty))),
            };

            foreach (var nullable in nullables)
            {
                nullable.Should().Throw<ArgumentException>();
            }
        }
    }
}
