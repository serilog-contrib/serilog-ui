using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using MsSql.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.Core.Models;
using Xunit;

namespace MsSql.Tests.DataProvider;

[Collection(nameof(MsSqlServerTestProvider))]
[Trait("Integration-Search", "MsSql")]
public class DataProviderSearchTest(MsSqlServerTestProvider instance) : IntegrationSearchTests<MsSqlServerTestProvider>(instance);

[Collection(nameof(MsSqlServerAdditionalColsTestProvider))]
[Trait("Integration-Search-AdditionalColumns", "MsSql")]
public class DataProviderSearchAdditionalColsTest(MsSqlServerAdditionalColsTestProvider instance)
    : IntegrationSearchTests<MsSqlServerAdditionalColsTestProvider>(instance)
{
    [Fact]
    public async Task It_finds_data_with_expected_additional_columns()
    {
        var query = new Dictionary<string, StringValues> { ["page"] = "1", ["count"] = "1000", ["search"] = "" };
        var res = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

        res.Item1.Should().HaveCount(LogCollector.DataSet.Count);
        res.Item1.As<IEnumerable<SqlServerTestModel>>().Should().AllSatisfy(it =>
        {
            it.Exception.Should().BeNullOrWhiteSpace();
            it.EnvironmentName.Should().NotBeNullOrWhiteSpace();
            it.EnvironmentUserName.Should().NotBeNullOrWhiteSpace();
        });
        
        var warningItems = res.Item1.Where(e => e.Level == "Warning").ToList();
        warningItems.First().As<SqlServerTestModel>().SampleBool.Should().BeTrue();
        warningItems.First().As<SqlServerTestModel>().SampleDate.Should().Be(new DateTime(2022, 01, 15, 09, 00, 00, DateTimeKind.Utc));
    }
}