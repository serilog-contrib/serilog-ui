using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.Web.Extensions;
using Xunit;

namespace Serilog.Ui.Web.Tests.Extensions;

[Trait("Ui-ServiceCollection", "Web")]
public class ServiceCollectionExtensionsTest
{
    [Fact]
    public void AddSerilogUi_registers_providers_options_in_services()
    {
        // Act
        var services = new ServiceCollection();
        services.AddSerilogUi(_ => { });

        // Assert
        var options = services.BuildServiceProvider().GetRequiredService<ProvidersOptions>();
        options.Should().NotBeNull();
    }

    [Fact]
    public void It_throws_if_AddSerilogUi_is_called_twice()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddSerilogUi(_ => { });
        var act = () => services.AddSerilogUi(_ => { });

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>().WithMessage("AddSerilogUi can be invoked one time per service registration.");
    }

}
