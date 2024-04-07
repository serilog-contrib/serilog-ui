using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using MySql.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.Core.Models;
using Xunit;

namespace MySql.Tests.DataProvider.MariaDb
{
    [Collection(nameof(MariaDbTestProvider))]
    [Trait("Integration-Search", "MariaDb")]
    public class DataProviderSearchTest(MariaDbTestProvider instance) : IntegrationSearchTests<MariaDbTestProvider>(instance);

    [Collection(nameof(MariaDbAdditionalColsTestProvider))]
    [Trait("Integration-Search-AdditionalColumns", "MariaDb")]
    public class DataProviderSearchAdditionalColsTest(MariaDbAdditionalColsTestProvider instance)
    : IntegrationSearchTests<MariaDbAdditionalColsTestProvider>(instance)
    {
        [Fact]
        public async Task It_finds_data_with_expected_additional_columns()
        {
            var query = new Dictionary<string, StringValues> { ["page"] = "1", ["count"] = "1000", ["search"] = "" };
            var res = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            res.Item1.Should().HaveCount(LogCollector.DataSet.Count);
            var sut = res.Item1.Cast<MariaDbTestModel>();
            sut.Should().AllSatisfy(it =>
            {
                it.Exception.Should().BeNullOrWhiteSpace();
                it.EnvironmentName.Should().NotBeNullOrWhiteSpace();
                it.EnvironmentUserName.Should().NotBeNullOrWhiteSpace();
            });

            var warningItems = sut.Where(e => e.Level == "Warning").ToList();
            warningItems.First().SampleBool.Should().Be("1");
            warningItems.First().SampleDate.Should().HaveDay(15).And.HaveYear(2022).And.HaveMonth(01);
        }
    }
}