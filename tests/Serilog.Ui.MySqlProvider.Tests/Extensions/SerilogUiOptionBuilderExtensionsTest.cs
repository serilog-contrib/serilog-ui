using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MySql.Tests.Util;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.MySqlProvider;
using Serilog.Ui.MySqlProvider.Extensions;
using Serilog.Ui.Web.Extensions;
using Serilog.Ui.Web.Models;
using Xunit;

namespace MySql.Tests.Extensions;

[Trait("DI-DataProvider", "MySql")]
public class SerilogUiOptionBuilderExtensionsMySqlTest
{
    private readonly ServiceCollection _serviceCollection = [];

    [Fact]
    public void It_registers_provider_and_dependencies()
    {
        _serviceCollection.AddSerilogUi(builder =>
        {
            builder.UseMySqlServer(opt => opt.WithConnectionString("https://mysqlserver.example.com").WithTable("my-table"));
        });

        var serviceProvider = _serviceCollection.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        var provider = scope.ServiceProvider.GetService<IDataProvider>();
        provider.Should().NotBeNull().And.BeOfType<MySqlDataProvider>();
    }

    [Fact]
    public void It_registers_multiple_providers()
    {
        _serviceCollection.AddSerilogUi(builder =>
        {
            builder
                .UseMySqlServer(opt => opt.WithConnectionString("https://sqlserver.example.com").WithTable("table"))
                .UseMySqlServer(opt => opt.WithConnectionString("https://sqlserver.example.com").WithTable("table-2"));
        });

        var serviceProvider = _serviceCollection.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        var providers = scope.ServiceProvider.GetServices<IDataProvider>().ToList();
        providers.Should().HaveCount(2).And.AllBeOfType<MySqlDataProvider>();
        providers.Select(p => p.Name).Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void It_throws_on_invalid_registration()
    {
        var nullables = new List<Func<IServiceCollection>>
        {
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMySqlServer(opt => opt.WithConnectionString(null!).WithTable("my-table"))),
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMySqlServer(opt => opt.WithConnectionString(" ").WithTable("my-table"))),
            () => _serviceCollection.AddSerilogUi(builder =>
                builder.UseMySqlServer(opt => opt.WithConnectionString(string.Empty).WithTable("my-table"))),
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMySqlServer(opt => opt.WithConnectionString("conn").WithTable(null!))),
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMySqlServer(opt => opt.WithConnectionString("conn").WithTable(" "))),
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMySqlServer(opt => opt.WithConnectionString("conn").WithTable(string.Empty))),
            // if user sets an invalid schema, default value will be overridden an validation should fail
            () => _serviceCollection.AddSerilogUi(builder =>
                builder.UseMySqlServer(opt => opt.WithConnectionString("conn").WithTable("ok").WithSchema(null!))),
            () => _serviceCollection.AddSerilogUi(builder =>
                builder.UseMySqlServer(opt => opt.WithConnectionString("conn").WithTable("ok").WithSchema(" "))),
            () => _serviceCollection.AddSerilogUi(builder =>
                builder.UseMySqlServer(opt => opt.WithConnectionString("conn").WithTable("ok").WithSchema(string.Empty))),
        };

        foreach (var nullable in nullables)
        {
            nullable.Should().Throw<ArgumentException>();
        }
    }
}

[Trait("DI-DataProvider", "MariaDb")]
public class SerilogUiOptionBuilderExtensionsMariaDbTest
{
    private readonly ServiceCollection _serviceCollection = [];

    [Fact]
    public void It_registers_provider_and_dependencies()
    {
        _serviceCollection.AddSerilogUi(builder =>
        {
            builder.UseMariaDbServer(opt => opt.WithConnectionString("https://mysqlserver.example.com").WithTable("my-table"));
        });

        var serviceProvider = _serviceCollection.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        var provider = scope.ServiceProvider.GetService<IDataProvider>();
        provider.Should().NotBeNull().And.BeOfType<MariaDbDataProvider>();
    }

    [Fact]
    public void It_registers_provider_and_dependencies_with_custom_log_model()
    {
        _serviceCollection.AddSerilogUi(builder =>
        {
            builder
                .UseMariaDbServer<MariaDbTestModel>(opt => opt.WithConnectionString("https://sqlserver.com").WithTable("table-custom"));
        });

        var serviceProvider = _serviceCollection.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        var provider = scope.ServiceProvider.GetService<IDataProvider>();
        provider.Should().NotBeNull().And.BeOfType<MariaDbDataProvider<MariaDbTestModel>>();
    }

    [Fact]
    public void It_registers_multiple_providers()
    {
        _serviceCollection.AddSerilogUi(builder =>
        {
            builder
                .UseMariaDbServer(opt => opt.WithConnectionString("https://sqlserver.com").WithTable("table"))
                .UseMariaDbServer(opt => opt.WithConnectionString("https://sqlserver.com").WithTable("table-2"))
                .UseMariaDbServer<MariaDbTestModel>(opt => opt.WithConnectionString("https://sqlserver.com").WithTable("table-custom"))
                .UseMariaDbServer<MariaDbTestModel>(opt => opt.WithConnectionString("https://sqlserver.com").WithTable("table-custom-2"));
        });

        var serviceProvider = _serviceCollection.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        var providers = scope.ServiceProvider.GetServices<IDataProvider>().ToList();
        providers.Should().HaveCount(4);
        providers.Take(2).Should().AllBeOfType<MariaDbDataProvider>();
        providers.Skip(2).Take(2).Should().AllBeOfType<MariaDbDataProvider<MariaDbTestModel>>();
        providers.Select(p => p.Name).Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void It_registers_additional_columns_options_during_service_registration()
    {
        var appBuilder = WebApplication.CreateBuilder();
        appBuilder.Services.AddSerilogUi(builder =>
        {
            builder
                .UseMariaDbServer(opt => opt
                    .WithConnectionString("https://sqlserver.com")
                    .WithTable("table"))
                .UseMariaDbServer<MariaDbTestModel>(opt => opt
                    .WithConnectionString("https://sqlserver.com")
                    .WithTable("table-custom"));
        });

        var app = appBuilder.Build();
        UiOptions? options = null;
        app.UseSerilogUi(opt => { options = opt; });

        options?.ColumnsInfo.Should().BeEquivalentTo(new Dictionary<string, ColumnsInfo>
        {
            ["MariaDb.dbo.table-custom"] = ColumnsInfo.Create<MariaDbTestModel>()
        });
    }

    [Fact]
    public void It_throws_on_invalid_registration()
    {
        var nullables = new List<Func<IServiceCollection>>
        {
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMariaDbServer(opt => opt.WithConnectionString(null!).WithTable("my-table"))),
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMariaDbServer(opt => opt.WithConnectionString(" ").WithTable("my-table"))),
            () => _serviceCollection.AddSerilogUi(builder =>
                builder.UseMariaDbServer(opt => opt.WithConnectionString(string.Empty).WithTable("my-table"))),
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMariaDbServer(opt => opt.WithConnectionString("conn").WithTable(null!))),
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMariaDbServer(opt => opt.WithConnectionString("conn").WithTable(" "))),
            () => _serviceCollection.AddSerilogUi(
                builder => builder.UseMariaDbServer(opt => opt.WithConnectionString("conn").WithTable(string.Empty))),
            // if user sets an invalid schema, default value will be overridden an validation should fail
            () => _serviceCollection.AddSerilogUi(builder =>
                builder.UseMariaDbServer(opt => opt.WithConnectionString("conn").WithTable("ok").WithSchema(null!))),
            () => _serviceCollection.AddSerilogUi(builder =>
                builder.UseMariaDbServer(opt => opt.WithConnectionString("conn").WithTable("ok").WithSchema(" "))),
            () => _serviceCollection.AddSerilogUi(builder =>
                builder.UseMariaDbServer(opt => opt.WithConnectionString("conn").WithTable("ok").WithSchema(string.Empty))),
        };

        foreach (var nullable in nullables)
        {
            nullable.Should().Throw<ArgumentException>();
        }
    }
}