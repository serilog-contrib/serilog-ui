using FluentAssertions;
using Serilog.Ui.Common.Tests.FakeObjectModels;
using Serilog.Ui.Core;
using Serilog.Ui.Core.OptionsBuilder;
using Xunit;

namespace Serilog.Ui.Web.Tests.OptionsBuilder;

[Trait("Unit-OptionsBuilder-ProvidersOptions", "Core")]
public class ProvidersOptionsTest
{
    private readonly ColumnsInfo _columnsInfoSample = ColumnsInfo.Create<TestLogModel>();

    [Fact]
    public void It_adds_entry_in_disabled_sort_keys()
    {
        ProvidersOptions.RegisterDisabledSortName("test");
        ProvidersOptions.RegisterDisabledSortName("test");
        ProvidersOptions.RegisterDisabledSortName("test2");

        ProvidersOptions.DisabledSortProviderNames.Should().BeEquivalentTo(["test", "test2"]);
    }

    [Fact]
    public void It_creates_entries_in_additional_columns()
    {
        ProvidersOptions.RegisterType<TestLogModel>("test");
        ProvidersOptions.RegisterType<TestLogModel>("test");
        ProvidersOptions.RegisterType<TestLogModel>("test2");

        ProvidersOptions.AdditionalColumns.Should().HaveCount(2);
        ProvidersOptions.AdditionalColumns.Should().ContainKeys("test", "test2");
        ProvidersOptions.AdditionalColumns.Values.Should().AllBeEquivalentTo(_columnsInfoSample);
    }

    [Fact]
    public void It_creates_info_from_log_model()
    {
        var result = ColumnsInfo.Create<TestLogModel>();

        result.RemovedColumns.Should().Contain("Exception", "Properties");
        result.AdditionalColumns.Should().BeEquivalentTo(new[]
        {
            new { Name = "SampleDate", TypeName = "datetime", CodeType = (CodeType?)null },
            new { Name = "SampleBool", TypeName = "boolean", CodeType = (CodeType?)null },
            new { Name = "EnvironmentName", TypeName = "string", CodeType = (CodeType?)CodeType.Json },
            new { Name = "EnvironmentUserName", TypeName = "string", CodeType = (CodeType?)null },
        });
    }

    [Fact]
    public void It_creates_info_from_default_log_model()
    {
        var resultDefaultClass = ColumnsInfo.Create<LogModel>();


        resultDefaultClass.RemovedColumns.Should().BeEmpty();
        resultDefaultClass.AdditionalColumns.Should().BeEmpty();
    }
}