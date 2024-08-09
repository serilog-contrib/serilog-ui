using FluentAssertions;
using Serilog.Ui.Common.Tests.FakeObjectModels;
using Serilog.Ui.Core.Attributes;
using Serilog.Ui.Core.Models;
using Xunit;

namespace Serilog.Ui.Web.Tests.OptionsBuilder;

[Trait("Unit-OptionsBuilder-ProvidersOptions", "Core")]
public class ProvidersOptionsTest
{
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