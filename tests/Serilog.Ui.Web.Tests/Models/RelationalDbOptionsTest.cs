using FluentAssertions;
using Serilog.Ui.Core.OptionsBuilder;
using Xunit;

namespace Serilog.Ui.Web.Tests.Models;

[Trait("Unit-RelationalDbOptions", "Core")]
public class RelationalDbOptionsTest
{
    [Fact]
    public void It_returns_custom_provider_name()
    {
        var result = new RelationalDbOptions("my-schema")
            .WithConnectionString("connection-string")
            .WithCustomProviderName("CUSTOM!");
        result.GetProviderName("Custom").Should().Be("CUSTOM!");
    }

    [Fact]
    public void It_returns_default_provider_name()
    {
        var result = new RelationalDbOptions("my-schema")
            .WithConnectionString("connection-string")
            .WithTable("table");

        result.GetProviderName("Custom").Should().Be("Custom.my-schema.table");
    }
}