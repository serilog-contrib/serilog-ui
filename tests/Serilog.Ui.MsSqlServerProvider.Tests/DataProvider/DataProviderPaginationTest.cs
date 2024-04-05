﻿using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using MsSql.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Serilog.Ui.Core.Models;
using Xunit;

namespace MsSql.Tests.DataProvider;

[Collection(nameof(MsSqlServerTestProvider))]
[Trait("Integration-Pagination", "MsSql")]
public class DataProviderPaginationTest(MsSqlServerTestProvider instance) : IntegrationPaginationTests<MsSqlServerTestProvider>(instance)
{
    [Fact]
    public override Task It_throws_when_skip_is_zero()
    {
        var query = new Dictionary<string, StringValues> { ["page"] = "0", ["count"] = "1" };
        var test = () => Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
        return test.Should().ThrowAsync<SqlException>();
    }
}

[Collection(nameof(MsSqlServerAdditionalColsTestProvider))]
[Trait("Integration-Pagination-AdditionalColumns", "MsSql")]
public class DataProviderPaginationAdditionalColsTest(MsSqlServerAdditionalColsTestProvider instance)
    : IntegrationPaginationTests<MsSqlServerAdditionalColsTestProvider>(instance)
{
    [Fact]
    public override Task It_throws_when_skip_is_zero()
    {
        var query = new Dictionary<string, StringValues> { ["page"] = "0", ["count"] = "1" };
        var test = () => Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
        return test.Should().ThrowAsync<SqlException>();
    }
}