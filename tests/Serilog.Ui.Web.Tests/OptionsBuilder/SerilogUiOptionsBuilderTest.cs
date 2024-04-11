using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Common.Tests.FakeObjectModels;
using Serilog.Ui.Core.OptionsBuilder;
using Xunit;

namespace Serilog.Ui.Web.Tests.OptionsBuilder;

[Trait("Unit-OptionsBuilder-SerilogUiOptionsBuilder", "Core")]
public class SerilogUiOptionsBuilderTest
{
    private readonly ColumnsInfo _columnsInfoSample = ColumnsInfo.Create<TestLogModel>();

    [Fact]
    public void It_adds_entry_in_disabled_sort_keys()
    {
        // Act
        var sut = GetBuilder();
        sut.RegisterDisabledSortForProviderKey("test");
        sut.RegisterDisabledSortForProviderKey("test");
        sut.RegisterDisabledSortForProviderKey("test2");

        // Assert
        sut.RegisterProviderServices();
        var provider = sut.Services.BuildServiceProvider();
        provider.GetRequiredService<ProvidersOptions>().DisabledSortProviderNames.Should().BeEquivalentTo(["test", "test2"]);
    }

    [Fact]
    public void It_creates_entries_in_additional_columns()
    {
        // Act
        var sut = GetBuilder();
        sut.RegisterColumnsInfo<TestLogModel>("test");
        sut.RegisterColumnsInfo<TestLogModel>("test");
        sut.RegisterColumnsInfo<TestLogModel>("test2");

        // Assert
        sut.RegisterProviderServices();
        var service = sut.Services.BuildServiceProvider().GetRequiredService<ProvidersOptions>();
        service.AdditionalColumns.Should().HaveCount(2);
        service.AdditionalColumns.Should().ContainKeys("test", "test2");
        service.AdditionalColumns.Values.Should().AllBeEquivalentTo(_columnsInfoSample);
    }

    [Fact]
    public void It_registers_options_in_services()
    {
        // Act
        var sut = GetBuilder();
        sut.RegisterProviderServices();

        // Assert
        sut.Services.BuildServiceProvider().GetRequiredService<ProvidersOptions>().Should().NotBeNull();
    }

    private static SerilogUiOptionsBuilder GetBuilder() => new(new ServiceCollection());
}
