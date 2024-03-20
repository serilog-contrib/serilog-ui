using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.MySqlProvider;
using Serilog.Ui.Web.Extensions;
using System;
using System.Collections.Generic;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.MySqlProvider.Extensions;
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