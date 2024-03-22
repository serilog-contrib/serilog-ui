using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.MySqlProvider;
using Serilog.Ui.Web.Extensions;
using System;
using System.Collections.Generic;
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
            builder.UseMySqlServer("https://mysqlserver.example.com", "my-table");
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
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMySqlServer(null, "name")),
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMySqlServer(" ", "name")),
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMySqlServer("", "name")),
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMySqlServer("name", null)),
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMySqlServer("name", " ")),
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMySqlServer("name", "")),
        };

        foreach (var nullable in nullables)
        {
            nullable.Should().ThrowExactly<ArgumentNullException>();
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
            builder.UseMariaDbServer("https://mysqlserver.example.com", "my-table");
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
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMariaDbServer(null, "name")),
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMariaDbServer(" ", "name")),
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMariaDbServer("", "name")),
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMariaDbServer("name", null)),
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMariaDbServer("name", " ")),
            () => _serviceCollection.AddSerilogUi(builder => builder.UseMariaDbServer("name", "")),
        };

        foreach (var nullable in nullables)
        {
            nullable.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}