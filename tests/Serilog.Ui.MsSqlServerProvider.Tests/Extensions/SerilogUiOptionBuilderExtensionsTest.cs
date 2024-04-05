using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.MsSqlServerProvider;
using Serilog.Ui.Web.Extensions;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using MsSql.Tests.Util;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.MsSqlServerProvider.Extensions;
using Serilog.Ui.Web.Models;
using Xunit;

namespace MsSql.Tests.Extensions
{
    [Trait("DI-DataProvider", "MsSql")]
    public class SerilogUiOptionBuilderExtensionsTest
    {
        private readonly ServiceCollection _serviceCollection = new();

        [Fact]
        public void It_registers_provider_and_dependencies()
        {
            _serviceCollection.AddSerilogUi(builder =>
            {
                builder.UseSqlServer(opt => opt
                    .WithConnectionString("https://sqlserver.example.com")
                    .WithTable("my-table"));
            });

            var serviceProvider = _serviceCollection.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var provider = scope.ServiceProvider.GetService<IDataProvider>();
            provider.Should().NotBeNull().And.BeOfType<SqlServerDataProvider<SqlServerLogModel>>();
        }

        [Fact]
        public void It_registers_provider_and_dependencies_with_custom_log_model()
        {
            _serviceCollection.AddSerilogUi(builder =>
            {
                builder.UseSqlServer<SqlServerTestModel>(opt => opt
                    .WithConnectionString("https://sqlserver.example.com")
                    .WithTable("my-table"));
            });

            var serviceProvider = _serviceCollection.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var provider = scope.ServiceProvider.GetService<IDataProvider>();
            provider.Should().NotBeNull().And.BeOfType<SqlServerDataProvider<SqlServerTestModel>>();
        }

        [Fact]
        public void It_registers_additional_columns_options_during_service_registration()
        {
            var appBuilder = WebApplication.CreateBuilder();
            appBuilder.Services.AddSerilogUi(builder =>
            {
                builder
                    .UseSqlServer(opt => opt
                        .WithConnectionString("https://sqlserver.example.com")
                        .WithTable("my-default-table"))
                    .UseSqlServer<SqlServerTestModel>(opt => opt
                        .WithConnectionString("https://sqlserver2.example.com")
                        .WithTable("my-table"));
            });

            var app = appBuilder.Build();
            UiOptions? options = null;
            app.UseSerilogUi(opt => { options = opt; });

            options?.ColumnsInfo.Should().BeEquivalentTo(new Dictionary<string, ColumnsInfo>
            {
                ["MsSQL.dbo.my-table"] = ColumnsInfo.Create<SqlServerTestModel>()
            });
        }

        [Fact]
        public void It_throws_on_invalid_registration()
        {
            var nullables = new List<Func<IServiceCollection>>
            {
                () => _serviceCollection.AddSerilogUi(builder => builder.UseSqlServer(opt =>
                    opt.WithConnectionString(null!).WithTable("my-table"))),
                () => _serviceCollection.AddSerilogUi(builder => builder.UseSqlServer(opt =>
                    opt.WithConnectionString(" ").WithTable("my-table"))),
                () => _serviceCollection.AddSerilogUi(builder => builder.UseSqlServer(opt =>
                    opt.WithConnectionString(string.Empty).WithTable("my-table"))),
                () => _serviceCollection.AddSerilogUi(builder => builder.UseSqlServer(opt =>
                    opt.WithConnectionString("name").WithTable(null!))),
                () => _serviceCollection.AddSerilogUi(builder => builder.UseSqlServer(opt =>
                    opt.WithConnectionString("name").WithTable(" "))),
                () => _serviceCollection.AddSerilogUi(builder => builder.UseSqlServer(opt =>
                    opt.WithConnectionString("name").WithTable(string.Empty))),
                // if user sets an invalid schema, default value will be overridden an validation should fail
                () => _serviceCollection.AddSerilogUi(builder =>
                    builder.UseSqlServer(opt => opt.WithConnectionString("conn").WithTable("ok").WithSchema(null!))),
                () => _serviceCollection.AddSerilogUi(builder =>
                    builder.UseSqlServer(opt => opt.WithConnectionString("conn").WithTable("ok").WithSchema(" "))),
                () => _serviceCollection.AddSerilogUi(builder =>
                    builder.UseSqlServer(opt => opt.WithConnectionString("conn").WithTable("ok").WithSchema(string.Empty))),
            };

            foreach (var nullable in nullables)
            {
                nullable.Should().Throw<ArgumentException>();
            }
        }
    }
}